namespace Kigg.Web
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Web.Configuration;
    using System.Web.Mvc;

    using Domain.Entities;
    using Infrastructure;
    using Repository;

    [OutputCache(CacheProfile = "FeedCache"), Compress]
    public class FeedController : BaseController
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IStoryRepository _storyRepository;

        public FeedController(IConfigurationManager configurationManager, ICategoryRepository categoryRepository, ITagRepository tagRepository, IStoryRepository storyRepository)
        {
            Check.Argument.IsNotNull(configurationManager, "configurationManager");
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");
            Check.Argument.IsNotNull(tagRepository, "tagRepository");
            Check.Argument.IsNotNull(storyRepository, "IStoryRepository");

            _configurationManager = configurationManager;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _storyRepository = storyRepository;
        }

        public ActionResult Published(string format, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            FeedViewData model = CreateViewData(start.Value, max.Value);
            model.Title = "{0} - Latest published stories".FormatWith(Settings.SiteTitle.HtmlEncode());
            model.Description = model.Title;
            model.Url = Url.RouteUrl("Published");

            try
            {
                PagedResult<Story> pagedStories = _storyRepository.FindPublished(start.Value, max.Value);

                model.Stories = pagedStories.Result;
                model.TotalStoryCount = pagedStories.Total;
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }

            return new FeedResult(model, format);
        }

        public ActionResult Category(string format, string name, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            name = name.NullSafe();

            FeedViewData model = CreateViewData(start.Value, max.Value);
            model.Title = "{0} - Latest published stories in {1}".FormatWith(Settings.SiteTitle.HtmlEncode(), name.HtmlEncode());
            model.Description = model.Title;

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    Category category = _categoryRepository.FindByUniqueName(name);

                    if (category != null)
                    {
                        PagedResult<Story> pagedStories = _storyRepository.FindPublishedByCategory(category.Id, start.Value, max.Value);

                        model.Stories = pagedStories.Result;
                        model.TotalStoryCount = pagedStories.Total;
                        name = category.UniqueName;
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            model.Url = Url.Action("Category", "Story", new { name });

            return new FeedResult(model, format);
        }

        public ActionResult Upcoming(string format, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            FeedViewData model = CreateViewData(start.Value, max.Value);

            model.Title = "{0} - Upcoming stories".FormatWith(Settings.SiteTitle.HtmlEncode());
            model.Description = model.Title;
            model.Url = Url.RouteUrl("Upcoming");

            try
            {
                PagedResult<Story> pagedStories = _storyRepository.FindUpcoming(start.Value, max.Value);

                model.Stories = pagedStories.Result;
                model.TotalStoryCount = pagedStories.Total;
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }

            return new FeedResult(model, format);
        }

        public ActionResult Tags(string format, string name, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            name = name.NullSafe();

            FeedViewData model = CreateViewData(start.Value, max.Value);
            model.Title = "{0} - Stories tagged with {1}".FormatWith(Settings.SiteTitle.HtmlEncode(), name.HtmlEncode());
            model.Description = model.Title;

            if (!string.IsNullOrEmpty(name))
            {
                try
                {
                    Tag tag = _tagRepository.FindByUniqueName(name) ?? _tagRepository.FindByName(name);

                    if (tag != null)
                    {
                        PagedResult<Story> pagedStories = _storyRepository.FindByTag(tag.Id, start.Value, max.Value);

                        model.Stories = pagedStories.Result;
                        model.TotalStoryCount = pagedStories.Total;
                        name = tag.UniqueName;
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            model.Url = Url.Action("Tags", "Story", new { name });

            return new FeedResult(model, format);
        }

        public ActionResult PromotedBy(string format, long userId, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            string userName = string.Empty;

            FeedViewData model = CreateViewData(start.Value, max.Value);

            if (userId > 0)
            {
                try
                {
                    User user = UserRepository.FindById(userId);

                    if (user != null)
                    {
                        userName = user.UserName;

                        PagedResult<Story> pagedStories = _storyRepository.FindPromotedByUser(user.Id, start.Value, max.Value);

                        model.Stories = pagedStories.Result;
                        model.TotalStoryCount = pagedStories.Total;
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            model.Title = "{0} - Stories promoted by {1}".FormatWith(Settings.SiteTitle.HtmlEncode(), userName.HtmlEncode());
            model.Description = model.Title;
            model.Url = Url.RouteUrl("User", new { userId, tab = UserDetailTab.Promoted });

            return new FeedResult(model, format);
        }

        public ActionResult PostedBy(string format, long userId, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            string userName = string.Empty;

            FeedViewData model = CreateViewData(start.Value, max.Value);

            if (userId > 0)
            {
                try
                {
                    User user = UserRepository.FindById(userId);

                    if (user != null)
                    {
                        userName = user.UserName;

                        PagedResult<Story> pagedStories = _storyRepository.FindPostedByUser(user.Id, start.Value, max.Value);

                        model.Stories = pagedStories.Result;
                        model.TotalStoryCount = pagedStories.Total;
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            model.Title = "{0} - Stories posted by {1}".FormatWith(Settings.SiteTitle.HtmlEncode(), userName.HtmlEncode());
            model.Description = model.Title;

            model.Url = Url.RouteUrl("User", new { userId, tab = UserDetailTab.Posted });

            return new FeedResult(model, format);
        }

        public ActionResult CommentedBy(string format, long userId, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            string userName = string.Empty;

            FeedViewData model = CreateViewData(start.Value, max.Value);

            if (userId > 0)
            {
                try
                {
                    User user = UserRepository.FindById(userId);

                    if (user != null)
                    {
                        userName = user.UserName;

                        PagedResult<Story> pagedStories = _storyRepository.FindCommentedByUser(user.Id, start.Value, max.Value);

                        model.Stories = pagedStories.Result;
                        model.TotalStoryCount = pagedStories.Total;
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            model.Title = "{0} - Stories commented by {1}".FormatWith(Settings.SiteTitle.HtmlEncode(), userName.HtmlEncode());
            model.Description = model.Title;
            model.Url = Url.RouteUrl("User", new { userId, tab = UserDetailTab.Commented });

            return new FeedResult(model, format);
        }

        public ActionResult Search(string format, string q, int? start, int? max)
        {
            EnsureInRange(ref start, ref max);

            q = q.NullSafe();

            FeedViewData model = CreateViewData(start.Value, max.Value);

            model.Title = "{0} - Search Result for {1}".FormatWith(Settings.SiteTitle.HtmlEncode(), q.HtmlEncode());
            model.Description = model.Title;
            model.Url = Url.RouteUrl("Search", new { q });

            if (!string.IsNullOrEmpty(q))
            {
                try
                {
                    PagedResult<Story> pagedStories = new PagedResult<Story>();
                        //_storyRepository.Search(q, start.Value, max.Value);

                    model.Stories = pagedStories.Result;
                    model.TotalStoryCount = pagedStories.Total;
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            return new FeedResult(model, format);
        }

        public void EnsureInRange(ref int? start, ref int? max)
        {
            if (!start.HasValue || (start <= 0))
            {
                start = 1;
            }

            start -= 1;

            if (!max.HasValue || (max <= 0))
            {
                max = Settings.FeedStoryPerPage;
            }

            if (max > 100)
            {
                max = 100;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public int GetCacheDuration()
        {
            int duration = 0;
            StackFrame frame = new StackFrame(2, false);

            MethodBase method = frame.GetMethod();

            // First try the Method attribute
            OutputCacheAttribute[] attributes = (OutputCacheAttribute[])method.GetCustomAttributes(typeof(OutputCacheAttribute), true);

            // If not found, then try class
            if (attributes.Length == 0)
            {
                attributes = (OutputCacheAttribute[])GetType().GetCustomAttributes(typeof(OutputCacheAttribute), true);
            }

            if (attributes.Length > 0)
            {
                OutputCacheAttribute cacheAttribute = attributes[0];

                if (cacheAttribute.Duration > 0)
                {
                    duration = cacheAttribute.Duration;
                }
                else
                {
                    if (!string.IsNullOrEmpty(cacheAttribute.CacheProfile))
                    {
                        OutputCacheSettingsSection settings = _configurationManager.GetSection<OutputCacheSettingsSection>("system.web/caching/outputCacheSettings");

                        if ((settings != null) && (settings.OutputCacheProfiles.Count > 0))
                        {
                            OutputCacheProfile profile = settings.OutputCacheProfiles.Get(cacheAttribute.CacheProfile);

                            if ((profile != null) && (profile.Duration > 0))
                            {
                                duration = profile.Duration;
                            }
                        }
                    }
                }
            }

            if (duration > 0)
            {
                duration = Convert.ToInt32(duration / 60);
            }

            return duration;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private FeedViewData CreateViewData(int start, int max)
        {
            return new FeedViewData
                       {
                           SiteTitle = Settings.SiteTitle,
                           RootUrl = Settings.RootUrl,
                           Email = Settings.WebmasterEmail,
                           PromoteText = Settings.PromoteText,
                           Start = start,
                           Max = max,
                           CacheDurationInMinutes = GetCacheDuration()
                       };
        }
    }
}