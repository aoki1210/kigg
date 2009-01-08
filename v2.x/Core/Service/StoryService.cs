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
        private readonly IUserScoreService _userScoreService;
        private readonly IDomainObjectFactory _factory;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IMarkAsSpamRepository _markAsSpamRepository;
        private readonly ISpamProtection _spamProtection;
        private readonly IEmailSender _emailSender;
        private readonly IContentService _contentService;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IThumbnail _thumbnail;

        private readonly IStoryWeightCalculator[] _storyWeightCalculators;

        public StoryService(IConfigurationSettings settings, IUserScoreService userScoreService, IDomainObjectFactory factory, ICategoryRepository categoryRepository, ITagRepository tagRepository, IStoryRepository storyRepository, IMarkAsSpamRepository markAsSpamRepository, ISpamProtection spamProtection, IEmailSender emailSender, IContentService contentService, IHtmlSanitizer htmlSanitizer, IThumbnail thumbnail, IStoryWeightCalculator[] storyWeightCalculators)
        {
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(userScoreService, "userScoreService");
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");
            Check.Argument.IsNotNull(tagRepository, "tagRepository");
            Check.Argument.IsNotNull(storyRepository, "storyRepository");
            Check.Argument.IsNotNull(markAsSpamRepository, "markAsSpamRepository");
            Check.Argument.IsNotNull(spamProtection, "spamProtection");
            Check.Argument.IsNotNull(emailSender, "emailSender");
            Check.Argument.IsNotNull(contentService, "contentService");
            Check.Argument.IsNotNull(htmlSanitizer, "htmlSanitizer");
            Check.Argument.IsNotNull(thumbnail, "thumbnail");
            Check.Argument.IsNotEmpty(storyWeightCalculators, "storyWeightCalculators");

            _settings = settings;
            _userScoreService = userScoreService;
            _factory = factory;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _storyRepository = storyRepository;
            _markAsSpamRepository = markAsSpamRepository;
            _spamProtection = spamProtection;
            _emailSender = emailSender;
            _contentService = contentService;
            _htmlSanitizer = htmlSanitizer;
            _thumbnail = thumbnail;
            _storyWeightCalculators = storyWeightCalculators;
        }

        public virtual StoryCreateResult Create(IUser byUser, string url, string title, string category, string description, string tags, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables, Func<IStory, string> buildDetailUrl)
        {
            StoryCreateResult result = ValidateCreate(byUser, url, title, category, description, userIPAddress, userAgent);

            if (result == null)
            {
                IStory alreadyExists = _storyRepository.FindByUrl(url);

                if (alreadyExists != null)
                {
                    return new StoryCreateResult { ErrorMessage = "Story with the same url already exists.", DetailUrl = buildDetailUrl(alreadyExists) };
                }

                ICategory storyCategory = _categoryRepository.FindByUniqueName(category);

                if (storyCategory == null)
                {
                    return new StoryCreateResult { ErrorMessage = "\"{0}\" category does not exist.".FormatWith(category) };
                }

                StoryContent content = _contentService.Get(url);

                if (content == StoryContent.Empty)
                {
                    return new StoryCreateResult { ErrorMessage = "Specified url appears to be a broken link." };
                }

                description = _htmlSanitizer.Sanitize(description);

                if (!_settings.AllowPossibleSpamStorySubmit)
                {
                    result = EnsureNotSpam<StoryCreateResult>(byUser, userIPAddress, userAgent, url, urlReferer, description, "social news", serverVariables, "Spam story rejected : {0}, {1}".FormatWith(url, byUser), "Your story appears to be a spam.");

                    if (result != null)
                    {
                        return result;
                    }
                }

                // If we are here which means story is not spam
                IStory story = _factory.CreateStory(storyCategory, byUser, userIPAddress, title.StripHtml(), description, url);

                _storyRepository.Add(story);

                // The Initial vote;
                story.Promote(story.CreatedAt, byUser, userIPAddress);

                // Capture the thumbnail, might speed up the thumbnail generation process
                _thumbnail.Capture(story.Url);

                // Subscribe comments by default
                story.SubscribeComment(byUser);

                AddTagsToContainers(tags, new ITagContainer[] { story, byUser });

                _userScoreService.StorySubmitted(byUser);

                string detailUrl = buildDetailUrl(story);

                if (_settings.AllowPossibleSpamStorySubmit && _settings.SendMailWhenPossibleSpamStorySubmitted)
                {
                    NotifyIfSpam(byUser, userIPAddress, userAgent, url, urlReferer, description, "social news", serverVariables, "Possible spam story submitted : {0}, {1}, {2}, {3}".FormatWith(detailUrl, story.Title, story.Url, byUser.UserName), () => _emailSender.NotifySpamStory(detailUrl, story));
                }

                // Ping the Story
                PingStory(content, story, detailUrl);

                result = new StoryCreateResult { NewStory = story, DetailUrl = detailUrl };
            }

            return result;
        }

        public virtual void Update(IStory theStory, string uniqueName, DateTime createdAt, string title, string category, string description, string tags)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

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

            if ((!string.IsNullOrEmpty(category)) && (string.Compare(category, theStory.BelongsTo.UniqueName, StringComparison.OrdinalIgnoreCase) != 0))
            {
                ICategory storyCategory = _categoryRepository.FindByUniqueName(category);
                theStory.ChangeCategory(storyCategory);
            }

            if (!string.IsNullOrEmpty(description))
            {
                theStory.HtmlDescription = description.Trim();
            }

            AddTagsToContainers(tags, new[] { theStory });
        }

        public virtual void Delete(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            _storyRepository.Remove(theStory);
            _emailSender.NotifyStoryDelete(theStory, byUser);
        }

        public virtual void View(IStory theStory, IUser byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotEmpty(fromIPAddress, "fromIPAddress");

            if (byUser != null)
            {
                _userScoreService.StoryViewed(theStory, byUser);
            }

            theStory.View(SystemTime.Now(), fromIPAddress);
        }

        public virtual void Promote(IStory theStory, IUser byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIPAddress, "fromIPAddress");

            if (theStory.Promote(SystemTime.Now(), byUser, fromIPAddress))
            {
                _userScoreService.StoryPromoted(theStory, byUser);
            }
        }

        public virtual void Demote(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (theStory.Demote(SystemTime.Now(), byUser))
            {
                _userScoreService.StoryDemoted(theStory, byUser);
            }
        }

        public virtual void MarkAsSpam(IStory theStory, string storyUrl, IUser byUser, string fromIPAddress)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIPAddress, "fromIPAddress");

            if (theStory.MarkAsSpam(SystemTime.Now(), byUser, fromIPAddress))
            {
                _userScoreService.StoryMarkedAsSpam(theStory, byUser);
            }

            _emailSender.NotifyStoryMarkedAsSpam(storyUrl, theStory, byUser);
        }

        public virtual void UnmarkAsSpam(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (theStory.UnmarkAsSpam(SystemTime.Now(), byUser))
            {
                _userScoreService.StoryUnmarkedAsSpam(theStory, byUser);
            }
        }

        public virtual CommentCreateResult Comment(IStory forStory, string storyUrl, IUser byUser, string content, bool subscribe, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables)
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

                IComment comment = forStory.PostComment(content, SystemTime.Now(), byUser, userIPAddress);

                if (subscribe)
                {
                    forStory.SubscribeComment(byUser);
                }
                else
                {
                    forStory.UnsubscribeComment(byUser);
                }

                _userScoreService.StoryCommented(forStory, byUser);

                // Notify the Comment Subscribers that a new comment is posted
                _emailSender.SendComment(storyUrl, comment, forStory.Subscribers);

                if (_settings.AllowPossibleSpamCommentSubmit && _settings.SendMailWhenPossibleSpamCommentSubmitted)
                {
                    NotifyIfSpam(byUser, userIPAddress, userAgent, storyUrl, urlReferer, content, "comment", serverVariables, "Possible spam comment submitted : {0}, {1}, {2}".FormatWith(storyUrl, forStory.Title, byUser.UserName), () => _emailSender.NotifySpamComment(storyUrl, comment));
                }

                result = new CommentCreateResult();
            }

            return result;
        }

        public virtual void Spam(IStory theStory, string storyUrl, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");

            if (!theStory.IsSpam())
            {
                theStory.Spam(SystemTime.Now());
                _userScoreService.StorySpammed(theStory.PostedBy);
            }

            _emailSender.NotifyConfirmSpamStory(storyUrl, theStory, byUser);
        }

        public virtual void Spam(IComment theComment, string storyUrl, IUser byUser)
        {
            Check.Argument.IsNotNull(theComment, "theComment");

            theComment.ForStory.DeleteComment(theComment);
            _userScoreService.CommentSpammed(theComment.ByUser);
            _emailSender.NotifyConfirmSpamComment(storyUrl, theComment, byUser);
        }

        public virtual void MarkAsOffended(IComment theComment, string storyUrl, IUser byUser)
        {
            Check.Argument.IsNotNull(theComment, "theComment");

            theComment.MarkAsOffended();
            _userScoreService.CommentMarkedAsOffended(theComment.ByUser);
            _emailSender.NotifyCommentAsOffended(storyUrl, theComment, byUser);
        }

        public virtual void Publish()
        {
            DateTime currentTime = SystemTime.Now();

            using (IUnitOfWork unitOfWork = UnitOfWork.Get())
            {
                IList<PublishedStory> publishableStories = GetPublishableStories(currentTime);

                if (!publishableStories.IsNullOrEmpty())
                {
                    // First penalty the user for marking the story as spam;
                    // It is obvious that the Moderator has already reviewed the story
                    // before it gets this far.
                    PenaltyUsersForIncorrectlyMarkingStoriesAsSpam(publishableStories);

                    //Then Publish the stories
                    PublishStories(currentTime, publishableStories);

                    // Commit every thing
                    unitOfWork.Commit();
                }
            }
        }

        private static SpamCheckContent CreateSpamCheckContent(IUser user, string userIpAddress, string userAgent, string url, string referer, string content, string type, NameValueCollection serverVariables)
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

        private static StoryCreateResult ValidateCreate(IUser byUser, string url, string title, string category, string description, string userIPAddress, string userAgent)
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

        private static CommentCreateResult ValidateComment(IStory forStory, IUser byUser, string content, string userIPAddress, string userAgent)
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

        private T EnsureNotSpam<T>(IUser byUser, string userIPAddress, string userAgent, string url, string urlReferer, string content, string contentType, NameValueCollection serverVariables, string logMessage, string errorMessage) where T : BaseServiceResult, new()
        {
            bool isSpam = _spamProtection.IsSpam(CreateSpamCheckContent(byUser, userIPAddress, userAgent, url, urlReferer, content, contentType, serverVariables));

            if (isSpam)
            {
                Log.Warning(logMessage);
            }

            return isSpam ? new T { ErrorMessage = errorMessage } : null;
        }

        private void NotifyIfSpam(IUser byUser, string userIPAddress, string userAgent, string url, string urlReferer, string content, string contentType, NameValueCollection serverVariables, string logMessage, Action sendEmail)
        {
            _spamProtection.IsSpam(
                                        CreateSpamCheckContent(byUser, userIPAddress, userAgent, url, urlReferer, content, contentType, serverVariables),
                                        isSpam =>
                                        {
                                            if (isSpam)
                                            {
                                                Log.Warning(logMessage);

                                                // Send Mail to Notify the Support that a Spam is submitted.
                                                sendEmail();
                                            }
                                        });
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
                            ITag tag = _tagRepository.FindByName(tagName);

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

        private void PingStory(StoryContent content, IStory story, string detailUrl)
        {
            if (_settings.SendPing && !string.IsNullOrEmpty(content.TrackBackUrl))
            {
                _contentService.Ping(content.TrackBackUrl, story.Title, detailUrl, "Thank you for submitting this cool story - Trackback from {0}".FormatWith(_settings.SiteTitle), _settings.SiteTitle);
            }
        }

        private IList<PublishedStory> GetPublishableStories(DateTime currentTime)
        {
            List<PublishedStory> publishableStories = new List<PublishedStory>();

            DateTime minimumDate = currentTime.AddHours(-_settings.MaximumAgeOfStoryInHoursToPublish);
            DateTime maximumDate = currentTime.AddHours(-_settings.MinimumAgeOfStoryInHoursToPublish);

            int publishableCount = _storyRepository.CountByPublishable(minimumDate, maximumDate);

            if (publishableCount > 0)
            {
                ICollection<IStory> stories = _storyRepository.FindPublishable(minimumDate, maximumDate, 0, publishableCount);

                foreach (IStory story in stories)
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
                ICollection<IMarkAsSpam> markedAsSpams = _markAsSpamRepository.FindAfter(publishableStory.Story.Id, publishableStory.Story.LastProcessedAt ?? publishableStory.Story.CreatedAt);

                foreach (IMarkAsSpam markedAsSpam in markedAsSpams)
                {
                    _userScoreService.StoryIncorrectlyMarkedAsSpam(markedAsSpam.ByUser);
                }
            }
        }

        private void PublishStories(DateTime currentTime, IList<PublishedStory> publishableStories)
        {
            // Now sort it based upon the score
            publishableStories = publishableStories.OrderByDescending(ps => ps.TotalScore).ToList();

            // Now assign the Rank
            publishableStories.ForEach(ps => ps.Rank = (publishableStories.IndexOf(ps) + 1));

            List<PublishedStory> publishingStories = new List<PublishedStory>();

            // Now Take stories that should fill the first page of each category
            ICollection<ICategory> categories = _categoryRepository.FindAll();

            foreach (ICategory category in categories)
            {
                Guid categoryId = category.Id;

                publishingStories.AddRange(publishableStories.Where(ps => ps.Story.BelongsTo.Id == categoryId).OrderBy(ps => ps.Rank).Take(_settings.HtmlStoryPerPage));
            }

            // Now take the stories for frontpage regardless its category
            IEnumerable<PublishedStory> frontPageStories = publishableStories.OrderBy(ps => ps.Rank).Take(_settings.HtmlStoryPerPage).AsEnumerable();
            publishingStories.AddRange(frontPageStories);

            if (publishingStories.Count > 0)
            {
                // Increase the User Score if the Story hits the frontpage
                foreach (PublishedStory ps in frontPageStories)
                {
                    _userScoreService.StoryPublished(ps.Story.PostedBy);
                }

                publishingStories = publishingStories.Distinct(new PublishedStoryComparer()).OrderBy(ps => ps.Rank).ToList();

                foreach (PublishedStory ps in publishingStories)
                {
                    ps.Story.Publish(currentTime, ps.Rank);
                }

                _emailSender.NotifyPublishedStories(currentTime, publishingStories);
            }

            // Mark the Story that it has been processed
            publishableStories.ForEach(ps => ps.Story.LastProcessed(currentTime));
        }

        private sealed class PublishedStoryComparer : IEqualityComparer<PublishedStory>
        {
            public bool Equals(PublishedStory x, PublishedStory y)
            {
                return x.Story.Id.Equals(y.Story.Id);
            }

            public int GetHashCode(PublishedStory obj)
            {
                return obj.Story.Id.GetHashCode();
            }
        }
    }
}