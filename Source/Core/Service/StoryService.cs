namespace Kigg.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class StoryService : IStoryService
    {
        private readonly IConfigurationSettings _settings;
        private readonly IDomainObjectFactory _factory;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly ISpamVoteRepository _markAsSpamRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISpamProtection _spamProtection;
        private readonly ISpamPostprocessor _spamPostprocessor;
        private readonly IContentService _contentService;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IThumbnail _thumbnail;
        private readonly IStoryWeightCalculator[] _storyWeightCalculators;

        public StoryService(IConfigurationSettings settings, IDomainObjectFactory factory, ICategoryRepository categoryRepository, ITagRepository tagRepository, IStoryRepository storyRepository, ISpamVoteRepository markAsSpamRepository, IEventAggregator eventAggregator, ISpamProtection spamProtection, ISpamPostprocessor spamPostprocessor, IContentService contentService, IHtmlSanitizer htmlSanitizer, IThumbnail thumbnail, IStoryWeightCalculator[] storyWeightCalculators)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");
            Check.Argument.IsNotNull(tagRepository, "tagRepository");
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(markAsSpamRepository, "markAsSpamRepository");
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");
            Check.Argument.IsNotNull(spamProtection, "spamProtection");
            Check.Argument.IsNotNull(spamPostprocessor, "spamPostprocessor");
            Check.Argument.IsNotNull(contentService, "contentService");
            Check.Argument.IsNotNull(htmlSanitizer, "htmlSanitizer");
            Check.Argument.IsNotNull(thumbnail, "thumbnail");
            Check.Argument.IsNotEmpty(storyWeightCalculators, "storyWeightCalculators");

            _settings = settings;
            _factory = factory;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _storyRepository = storyRepository;
            _markAsSpamRepository = markAsSpamRepository;
            _eventAggregator = eventAggregator;
            _spamProtection = spamProtection;
            _spamPostprocessor = spamPostprocessor;
            _contentService = contentService;
            _htmlSanitizer = htmlSanitizer;
            _thumbnail = thumbnail;
            _storyWeightCalculators = storyWeightCalculators;
        }

        public virtual StoryCreateResult Create(User byUser, string url, string title, string category, string description, string tags, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables, Func<Story, string> buildDetailUrl)
        {
            StoryCreateResult result = ValidateCreate(byUser, url, title, category, description, userIPAddress, userAgent);

            if (result == null)
            {
                if (_contentService.IsRestricted(url))
                {
                    result = new StoryCreateResult { ErrorMessage = "Specifed url has match with our banned url list." };
                }
            }

            if (result == null)
            {
                using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                {
                    Story alreadyExists = _storyRepository.FindByUrl(url);

                    if (alreadyExists != null)
                    {
                        return new StoryCreateResult { ErrorMessage = "Story with the same url already exists.", DetailUrl = buildDetailUrl(alreadyExists) };
                    }

                    Category storyCategory = _categoryRepository.FindByUniqueName(category);

                    if (storyCategory == null)
                    {
                        return new StoryCreateResult { ErrorMessage = "\"{0}\" category does not exist.".FormatWith(category) };
                    }

                    StoryContent content = _contentService.Get(url);

                    if (content == StoryContent.Empty)
                    {
                        return new StoryCreateResult { ErrorMessage = "Specified url appears to be broken." };
                    }

                    description = _htmlSanitizer.Sanitize(description);

                    if (!_settings.AllowPossibleSpamStorySubmit && ShouldCheckSpamForUser(byUser))
                    {
                        result = EnsureNotSpam<StoryCreateResult>(byUser, userIPAddress, userAgent, url, urlReferer, description, "social news", serverVariables, "Spam story rejected : {0}, {1}".FormatWith(url, byUser), "Your story appears to be a spam.");

                        if (result != null)
                        {
                            return result;
                        }
                    }

                    // If we are here which means story is not spam
                    Story story = _factory.CreateStory(storyCategory, byUser, userIPAddress, title.StripHtml(), description, url);

                    _storyRepository.Add(story);

                    // The Initial vote;
                    story.Promote(story.CreatedAt, byUser, userIPAddress);

                    // Capture the thumbnail, might speed up the thumbnail generation process
                    _thumbnail.Capture(story.Url);

                    // Subscribe comments by default
                    story.SubscribeComment(byUser);

                    AddTagsToContainers(tags, new ITagContainer[] { story, byUser });

                    string detailUrl = buildDetailUrl(story);

                    if (_settings.AllowPossibleSpamStorySubmit && _settings.SendMailWhenPossibleSpamStorySubmitted && ShouldCheckSpamForUser(byUser))
                    {
                        unitOfWork.Commit();
                        _spamProtection.IsSpam(CreateSpamCheckContent(byUser, userIPAddress, userAgent, url, urlReferer, description, "social news", serverVariables), (source, isSpam) => _spamPostprocessor.Process(source, isSpam, detailUrl, story));
                    }
                    else
                    {
                        story.Approve(SystemTime.Now());
                        _eventAggregator.GetEvent<StorySubmitEvent>().Publish(new StorySubmitEventArgs(story, detailUrl));
                        unitOfWork.Commit();
                    }

                    result = new StoryCreateResult { NewStory = story, DetailUrl = detailUrl };
                }
            }

            return result;
        }

        public virtual void Update(Story theStory, string uniqueName, DateTime createdAt, string title, string category, string description, string tags)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

            using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                if (string.IsNullOrEmpty(uniqueName))
                {
                    uniqueName = theStory.UniqueName;
                }

                if (!createdAt.IsValid())
                {
                    createdAt = theStory.CreatedAt;
                }

                theStory.ChangeNameAndCreatedAt(uniqueName, createdAt);

                if (!string.IsNullOrEmpty(title))
                {
                    theStory.Title = title;
                }

                if ((!string.IsNullOrEmpty(category)) &&
                    (string.Compare(category, theStory.BelongsTo.UniqueName, StringComparison.OrdinalIgnoreCase) != 0))
                {
                    Category storyCategory = _categoryRepository.FindByUniqueName(category);
                    theStory.ChangeCategory(storyCategory);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    theStory.HtmlDescription = description.Trim();
                }

                AddTagsToContainers(tags, new[] { theStory });

                unitOfWork.Commit();
            }
        }

        public virtual void Delete(Story theStory, User byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                //Removing story is supposed to be before Publishing Delete Event. However, because that Entity Framework
                //set all related objects to null, a NullReferenceException will occure within event subscribers, as they
                //might access related objects such as PostedBy, BelongsTo, Votes etc...
                _eventAggregator.GetEvent<StoryDeleteEvent>().Publish(new StoryDeleteEventArgs(theStory, byUser));

                _storyRepository.Remove(theStory);

                unitOfWork.Commit();
            }
        }

        public virtual void View(Story theStory, User byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNullOrEmpty(fromIPAddress, "fromIPAddress");

            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                theStory.View(SystemTime.Now(), fromIPAddress);

                _eventAggregator.GetEvent<StoryViewEvent>().Publish(new StoryViewEventArgs(theStory, byUser));

                unitOfWork.Commit();
            }
        }

        public virtual void Promote(Story theStory, User byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotNullOrEmpty(fromIPAddress, "fromIPAddress");

            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                if (theStory.Promote(SystemTime.Now(), byUser, fromIPAddress))
                {
                    _eventAggregator.GetEvent<StoryPromoteEvent>().Publish(new StoryPromoteEventArgs(theStory, byUser));

                    unitOfWork.Commit();
                }
            }
        }

        public virtual void Demote(Story theStory, User byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                if (theStory.Demote(SystemTime.Now(), byUser))
                {
                    _eventAggregator.GetEvent<StoryDemoteEvent>().Publish(new StoryDemoteEventArgs(theStory, byUser));

                    unitOfWork.Commit();
                }
            }
        }

        public virtual void MarkAsSpam(Story theStory, string storyUrl, User byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNullOrEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotNullOrEmpty(fromIPAddress, "fromIPAddress");

            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                if (theStory.MarkAsSpam(SystemTime.Now(), byUser, fromIPAddress))
                {
                    _eventAggregator.GetEvent<StoryMarkAsSpamEvent>().Publish(new StoryMarkAsSpamEventArgs(theStory, byUser, storyUrl));

                    unitOfWork.Commit();
                }
            }
        }

        public virtual void UnmarkAsSpam(Story theStory, User byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                if (theStory.UnmarkAsSpam(SystemTime.Now(), byUser))
                {
                    _eventAggregator.GetEvent<StoryUnmarkAsSpamEvent>().Publish(new StoryUnmarkAsSpamEventArgs(theStory, byUser));

                    unitOfWork.Commit();
                }
            }
        }

        public virtual CommentCreateResult Comment(Story forStory, string storyUrl, User byUser, string content, bool subscribe, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables)
        {
            CommentCreateResult result = ValidateComment(forStory, byUser, content, userIPAddress, userAgent);

            if (result == null)
            {
                content = SanitizeHtml(content);

                if (!_settings.AllowPossibleSpamCommentSubmit)
                {
                    result = EnsureNotSpam<CommentCreateResult>(byUser, userIPAddress, userAgent, storyUrl, urlReferer, content, "comment", serverVariables, "Possible spam rejected : {0}, {1}, {2}".FormatWith(storyUrl, forStory.Title, byUser), "Your comment appears to be a spam.");

                    if (result != null)
                    {
                        return result;
                    }
                }

                using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
                {
                    Comment comment = forStory.PostComment(content, SystemTime.Now(), byUser, userIPAddress);

                    if (subscribe)
                    {
                        forStory.SubscribeComment(byUser);
                    }
                    else
                    {
                        forStory.UnsubscribeComment(byUser);
                    }

                    if (_settings.AllowPossibleSpamCommentSubmit && _settings.SendMailWhenPossibleSpamCommentSubmitted)
                    {
                        unitOfWork.Commit();
                        _spamProtection.IsSpam(CreateSpamCheckContent(byUser, userIPAddress, userAgent, storyUrl, urlReferer, content, "comment", serverVariables), (source, isSpam) => _spamPostprocessor.Process(source, isSpam, storyUrl, comment));
                    }
                    else
                    {
                        _eventAggregator.GetEvent<CommentSubmitEvent>().Publish(new CommentSubmitEventArgs(comment, storyUrl));
                        unitOfWork.Commit();
                    }

                    result = new CommentCreateResult();
                }
            }

            return result;
        }

        public virtual void Spam(Story theStory, string storyUrl, User byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNullOrEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (!theStory.IsPublished())
            {
                using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
                {
                    //Removing story is supposed to be before Publishing Spam Event. However, because that Entity Framework
                    //set all related objects to null, a NullReferenceException will occure within event subscribers, as they
                    //might access related objects such as PostedBy, BelongsTo, Votes etc...
                    _eventAggregator.GetEvent<StorySpamEvent>().Publish(new StorySpamEventArgs(theStory, byUser, storyUrl));
                    
                    _storyRepository.Remove(theStory);

                    unitOfWork.Commit();
                }
            }
        }

        public virtual void Spam(Comment theComment, string storyUrl, User byUser)
        {
            Check.Argument.IsNotNull(theComment, "theComment");

            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                //Removing story is supposed to be before Publishing Spam Event. However, because that Entity Framework
                //set all related objects to null, a NullReferenceException will occure within event subscribers, as they
                //might access related objects such as PostedBy etc...
                _eventAggregator.GetEvent<CommentSpamEvent>().Publish(new CommentSpamEventArgs(theComment, byUser, storyUrl));
                
                theComment.ForStory.DeleteComment(theComment);

                unitOfWork.Commit();
            }
        }

        public virtual void MarkAsOffended(Comment theComment, string storyUrl, User byUser)
        {
            Check.Argument.IsNotNull(theComment, "theComment");
            Check.Argument.IsNotNullOrEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (!theComment.IsOffended)
            {
                using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
                {
                    theComment.MarkAsOffended();

                    _eventAggregator.GetEvent<CommentMarkAsOffendedEvent>().Publish(new CommentMarkAsOffendedEventArgs(theComment, byUser, storyUrl));

                    unitOfWork.Commit();
                }
            }
        }

        public virtual void Publish()
        {
            using(IUnitOfWork unitOfWork = UnitOfWork.Begin())
            {
                DateTime currentTime = SystemTime.Now();

                IList<PublishedStory> publishableStories = GetPublishableStories(currentTime);

                if (!publishableStories.IsNullOrEmpty())
                {
                    // First penalty the user for marking the story as spam;
                    // It is obvious that the Moderator has already reviewed the story
                    // before it gets this far.
                    PenaltyUsersForIncorrectlyMarkingStoriesAsSpam(publishableStories);

                    //Then Publish the stories
                    PublishStories(currentTime, publishableStories);

                    unitOfWork.Commit();
                }
            }
        }

        public virtual void Approve(Story theStory, string storyUrl, User byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNullOrEmpty(storyUrl, "storyUrl");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (!theStory.IsApproved())
            {
                using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                {
                    theStory.Approve(SystemTime.Now());

                    _eventAggregator.GetEvent<StoryApproveEvent>().Publish(new StoryApproveEventArgs(theStory, byUser, storyUrl));

                    unitOfWork.Commit();
                }
            }
        }

        private static SpamCheckContent CreateSpamCheckContent(User user, string userIpAddress, string userAgent, string url, string referer, string content, string type, NameValueCollection serverVariables)
        {
            SpamCheckContent checkContentToCheck = new SpamCheckContent
                                             {
                                                 UserIPAddress = userIpAddress,
                                                 UserAgent = userAgent,
                                                 UserName = user.UserName,
                                                 Url = url,
                                                 UrlReferer = referer,
                                                 Content = content,
                                                 ContentType = type
                                             };

            if (serverVariables != null)
            {
                checkContentToCheck.Extra.Add(serverVariables);
            }

            return checkContentToCheck;
        }

        private static StoryCreateResult ValidateCreate(User byUser, string url, string title, string category, string description, string userIPAddress, string userAgent)
        {
            StoryCreateResult result = null;

            if (byUser == null)
            {
                result = new StoryCreateResult { ErrorMessage = "User cannot be null." };
            }
            else if (string.IsNullOrEmpty(url))
            {
                result = new StoryCreateResult { ErrorMessage = "Url cannot be blank." };
            }
            else if (!url.IsWebUrl())
            {
                result = new StoryCreateResult { ErrorMessage = "Invalid web url." };
            }
            else if (string.IsNullOrEmpty(title))
            {
                result = new StoryCreateResult { ErrorMessage = "Title cannot be blank." };
            }
            else if (title.Trim().Length > 256)
            {
                result = new StoryCreateResult { ErrorMessage = "Title cannot be more than 256 character." };
            }
            else if (string.IsNullOrEmpty(category))
            {
                result = new StoryCreateResult { ErrorMessage = "Category cannot be blank." };
            }
            else if (string.IsNullOrEmpty(description))
            {
                result = new StoryCreateResult { ErrorMessage = "Description cannot be blank." };
            }
            else if ((description.Trim().Length < 8) || (description.Trim().Length > 2048))
            {
                result = new StoryCreateResult { ErrorMessage = "Description must be between 8 to 2048 character." };
            }
            else if (string.IsNullOrEmpty(userIPAddress))
            {
                result = new StoryCreateResult { ErrorMessage = "User Ip address cannot be blank." };
            }
            else if (string.IsNullOrEmpty(userAgent))
            {
                result = new StoryCreateResult { ErrorMessage = "User agent cannot be empty." };
            }

            return result;
        }

        private static CommentCreateResult ValidateComment(Story forStory, User byUser, string content, string userIPAddress, string userAgent)
        {
            CommentCreateResult result = null;

            if (forStory == null)
            {
                result = new CommentCreateResult { ErrorMessage = "Story cannot be null." };
            }
            else if (byUser == null)
            {
                result = new CommentCreateResult { ErrorMessage = "User cannot be null." };
            }
            else if (string.IsNullOrEmpty(content))
            {
                result = new CommentCreateResult { ErrorMessage = "Comment cannot be blank." };
            }
            else if (content.Trim().Length > 2048)
            {
                result = new CommentCreateResult { ErrorMessage = "Comment cannot be more than 2048 character." };
            }
            else if (string.IsNullOrEmpty(userIPAddress))
            {
                result = new CommentCreateResult { ErrorMessage = "User ip address cannot be blank." };
            }
            else if (string.IsNullOrEmpty(userAgent))
            {
                result = new CommentCreateResult { ErrorMessage = "User agent cannot be empty." };
            }

            return result;
        }

        private bool ShouldCheckSpamForUser(User user)
        {
            return !user.CanModerate() && (_storyRepository.CountPostedByUser(user.Id) <= _settings.StorySumittedThresholdOfUserToSpamCheck);
        }

        private T EnsureNotSpam<T>(User byUser, string userIPAddress, string userAgent, string url, string urlReferer, string content, string contentType, NameValueCollection serverVariables, string logMessage, string errorMessage) where T : BaseServiceResult, new()
        {
            bool isSpam = _spamProtection.IsSpam(CreateSpamCheckContent(byUser, userIPAddress, userAgent, url, urlReferer, content, contentType, serverVariables));

            if (isSpam)
            {
                Log.Warning(logMessage);
            }

            return isSpam ? new T { ErrorMessage = errorMessage } : null;
        }

        private string SanitizeHtml(string html)
        {
            return _htmlSanitizer.Sanitize(html);
        }

        private void AddTagsToContainers(string tags, IEnumerable<ITagContainer> tagContainers)
        {
            if (!string.IsNullOrEmpty(tags))
            {
                string[] tagNames = tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (tagNames.Length > 0)
                {
                    foreach (string t in tagNames)
                    {
                        string tagName = t.NullSafe().StripHtml();

                        if (!string.IsNullOrEmpty(tagName))
                        {
                            Tag tag = _tagRepository.FindByName(tagName);

                            if (tag == null)
                            {
                                tag = _factory.CreateTag(tagName);
                                _tagRepository.Add(tag);
                            }

                            foreach (ITagContainer container in tagContainers)
                            {
                                container.AddTag(tag);
                            }
                        }
                    }
                }
            }
        }

        private IList<PublishedStory> GetPublishableStories(DateTime currentTime)
        {
            List<PublishedStory> publishableStories = new List<PublishedStory>();

            DateTime minimumDate = currentTime.AddHours(-_settings.MaximumAgeOfStoryInHoursToPublish);
            DateTime maximumDate = currentTime.AddHours(-_settings.MinimumAgeOfStoryInHoursToPublish);

            int publishableCount = _storyRepository.CountPublishable(minimumDate, maximumDate);

            if (publishableCount > 0)
            {
                ICollection<Story> stories = _storyRepository.FindPublishable(minimumDate, maximumDate, 0, publishableCount).Result;

                foreach (Story story in stories)
                {
                    PublishedStory publishedStory = new PublishedStory(story);

                    foreach (IStoryWeightCalculator strategy in _storyWeightCalculators)
                    {
                        publishedStory.Weights.Add(strategy.Name, strategy.Calculate(currentTime, story));
                    }

                    publishableStories.Add(publishedStory);
                }
            }

            return publishableStories;
        }

        private void PenaltyUsersForIncorrectlyMarkingStoriesAsSpam(IEnumerable<PublishedStory> publishableStories)
        {
            foreach(PublishedStory publishableStory in publishableStories)
            {
                IEnumerable<SpamVote> markedAsSpams = _markAsSpamRepository.FindAfter(publishableStory.Story.Id, publishableStory.Story.LastProcessedAt ?? publishableStory.Story.CreatedAt);

                foreach (SpamVote markedAsSpam in markedAsSpams)
                {
                    _eventAggregator.GetEvent<StoryIncorrectlyMarkedAsSpamEvent>().Publish(new StoryIncorrectlyMarkedAsSpamEventArgs(publishableStory.Story, markedAsSpam.ByUser));
                }
            }
        }

        private void PublishStories(DateTime currentTime, IList<PublishedStory> publishableStories)
        {
            // Now sort it based upon the score
            publishableStories = publishableStories.OrderByDescending(ps => ps.TotalScore).ToList();

            // Now assign the Rank
            publishableStories.ForEach(ps => ps.Rank = (publishableStories.IndexOf(ps) + 1));

            // Now take the stories for front page
            ICollection<PublishedStory> frontPageStories = publishableStories.OrderBy(ps => ps.Rank).Take(_settings.HtmlStoryPerPage).ToList();

            if (!frontPageStories.IsNullOrEmpty())
            {
                foreach (PublishedStory ps in frontPageStories)
                {
                    //Increase user score
                    //_userScoreService.StoryPublished(ps.Story);

                    ps.Story.Publish(currentTime, ps.Rank);
                }

                _eventAggregator.GetEvent<StoryPublishEvent>().Publish(new StoryPublishEventArgs(frontPageStories, currentTime));
            }

            // Mark the Story that it has been processed
            publishableStories.ForEach(ps => ps.Story.LastProcessed(currentTime));
        }
    }
}