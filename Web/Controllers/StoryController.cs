namespace Kigg
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web.Security;

    /// <summary>
    /// Handles all story related operation.
    /// </summary>
    public class StoryController : BaseController, IDisposable
    {
        private static readonly Regex UrlExpression = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly int _storyPerPage = 10;
        private static readonly int _topTags = 50;
        private static readonly int _qualifyingKigg = 3;

        private readonly IDataContext _dataContext;

        private IDataContext DataContext
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return _dataContext;
            }
        }

        /// <summary>
        /// Initializes the <see cref="StoryController"/> class.
        /// </summary>
        static StoryController()
        {
            var settings = ConfigurationManager.GetSection("storySettings") as Hashtable;

            if (settings != null)
            {
                _storyPerPage = Convert.ToInt32(settings["storyPerPage"], CultureInfo.InvariantCulture);
                _topTags = Convert.ToInt32(settings["topTags"], CultureInfo.InvariantCulture);
                _qualifyingKigg = Convert.ToInt32(settings["qualifyingKigg"], CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryController"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="userManager">The membership provider that will be used.</param>
        public StoryController(IDataContext dataContext, MembershipProvider userManager):base(userManager)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryController"/> class.
        /// </summary>
        public StoryController():this(new KiggDataContext(), Membership.Provider)
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }

        /// <summary>
        /// List published stories of both all category and specific category
        /// </summary>
        /// <param name="name">The category name. If the parameter is blank it shows all the stories regarless the category.</param>
        /// <param name="page">The page number (1 based). If not specifed then shows the first page.</param>
        public void Category(string name, int? page)
        {
            using (CodeBenchmark.Start)
            {
                var viewData = GetStoryListViewData<StoryListByCategoryData>(page);

                var total = 0;

                if (string.IsNullOrEmpty(name))
                {
                    viewData.Stories = DataContext.GetPublishedStoriesForAllCategory(CurrentUserId, CalculateStartIndex(page), _storyPerPage, out total);
                    name = "All";
                }
                else
                {
                    name = name.UrlDecode();

                    var category = DataContext.GetCategoryByName(name);

                    if (category != null)
                    {
                        viewData.Stories = DataContext.GetPublishedStoriesForCategory(CurrentUserId, category.ID, CalculateStartIndex(page), _storyPerPage, out total);
                    }
                }

                viewData.Category = name;
                viewData.StoryCount = total;

                RenderView("Category", viewData);
            }
        }

        /// <summary>
        /// List all upcoming stories regardless the category
        /// </summary>
        /// <param name="page">The page number (1 based). If not specifed then shows the first page.</param>
        public void Upcoming(int? page)
        {
            using (CodeBenchmark.Start)
            {
                var viewData = GetStoryListViewData<StoryListByCategoryData>(page);
                int total;

                viewData.Stories = DataContext.GetUpcomingStories(CurrentUserId, CalculateStartIndex(page), _storyPerPage, out total);
                viewData.StoryCount = total;
                viewData.Category = "Upcoming";

                RenderView("Category", viewData);
            }
        }

        /// <summary>
        /// List Stories for a specific tag
        /// </summary>
        /// <param name="name">The tag name. If the parameter is blank it redirect to category action.</param>
        /// <param name="page">The page number (1 based). If not specifed then shows the first page.</param>
        public void Tag(string name, int? page)
        {
            if (string.IsNullOrEmpty(name))
            {
                RedirectToAction("Category");
                return;
            }

            name = name.UrlDecode();

            using (CodeBenchmark.Start)
            {
                var viewData = GetStoryListViewData<StoryListByTagData>(page);

                Tag tag = DataContext.GetTagByName(name);
                int total = 0;

                if (tag != null)
                {
                    viewData.Stories = DataContext.GetStoriesForTag(CurrentUserId, tag.ID, CalculateStartIndex(page), _storyPerPage, out total);
                }

                viewData.StoryCount = total;
                viewData.Tag = name;

                RenderView("Tag", viewData);
            }
        }

        /// <summary>
        /// List Stories Posted by a Specific User
        /// </summary>
        /// <param name="name">The name of the user. If the parameter is blank it redirect to category action.</param>
        /// <param name="page">The page number (1 based). If not specifed then shows the first page.</param>
        public void PostedBy(string name, int? page)
        {
            if (string.IsNullOrEmpty(name))
            {
                RedirectToAction("Category");
                return;
            }

            name = name.UrlDecode();

            using (CodeBenchmark.Start)
            {
                var viewData = GetStoryListViewData<StoryListByUserData>(page);
                int total = 0;

                MembershipUser user = UserManager.GetUser(name, false);

                if (user != null)
                {
                    viewData.Stories = DataContext.GetStoriesPostedByUser(CurrentUserId, (Guid)user.ProviderUserKey, CalculateStartIndex(page), _storyPerPage, out total);
                }

                viewData.StoryCount = total;
                viewData.PostedBy = name;

                RenderView("PostedBy", viewData);
            }
        }

        /// <summary>
        /// Search the Stories
        /// </summary>
        /// <param name="q">The Search Query</param>
        /// <param name="page">The page number (1 based). If not specifed then shows the first page.</param>
        public void Search(string q, int? page)
        {
            if (string.IsNullOrEmpty(q))
            {
                RedirectToAction("Category");
                return;
            }

            q = q.UrlDecode();

            using (CodeBenchmark.Start)
            {
                var viewData = GetStoryListViewData<StoryListBySearchData>(page);
                int total;

                viewData.Stories = DataContext.SearchStories(CurrentUserId, q, CalculateStartIndex(page), _storyPerPage, out total);
                viewData.StoryCount = total;
                viewData.SearchQuery = q;

                RenderView("Search", viewData);
            }
        }

        /// <summary>
        /// View the detail of the specified story.
        /// </summary>
        /// <param name="id">The story id (Mandatory).</param>
        public void Detail(int id)
        {
            using (CodeBenchmark.Start)
            {
                var viewData = GetViewData<StoryDetailData>();

                viewData.Story = DataContext.GetStoryDetailById(CurrentUserId, id);

                RenderView("Detail", viewData);
            }
        }

        /// <summary>
        /// Submits a Story.The user must be authenticated prior calling this method.
        /// This is an Ajax Operation. It does not accept duplicate urls.
        /// </summary>
        /// <param name="storyUrl">The story URL (Mandatory).</param>
        /// <param name="storyTitle">The story title (Mandatory).</param>
        /// <param name="storyCategoryId">The story category id (Mandatory).</param>
        /// <param name="storyDescription">The story description (Mandatory).</param>
        /// <param name="storyTags">The story tags (Optional).</param>
        public void Submit(string storyUrl, string storyTitle, int storyCategoryId, string storyDescription, string storyTags)
        {
            using (CodeBenchmark.Start)
            {
                var result = new JsonResult();

                if (!IsUserAuthenticated)
                {
                    result.errorMessage = "You are not authenticated to call this method.";
                }
                else if (string.IsNullOrEmpty(storyUrl))
                {
                    result.errorMessage = "Story url cannot be blank.";
                }
                else if (!IsValidUrl(storyUrl))
                {
                    result.errorMessage = "Invalid url.";
                }
                else if (string.IsNullOrEmpty(storyTitle))
                {
                    result.errorMessage = "Story title cannot be blank.";
                }
                else if (string.IsNullOrEmpty(storyDescription))
                {
                    result.errorMessage = "Story description cannot be blank.";
                }
                else
                {
                    try
                    {
                        DataContext.SubmitStory(storyUrl, storyTitle, storyCategoryId, storyDescription, storyTags, CurrentUserId);
                        result.isSuccessful = true;
                    }
                    catch (InvalidOperationException e)
                    {
                        result.errorMessage = e.Message;
                    }
                }

                RenderView("Json", result);
            }
        }

        /// <summary>
        /// Kigg the Story. The user must be authenticated prior calling this method.
        /// This is an Ajax Operation.
        /// </summary>
        /// <param name="storyId">The story id(Mandatory).</param>
        public void Kigg(int storyId)
        {
            using (CodeBenchmark.Start)
            {
                var result = new JsonResult();

                if (!IsUserAuthenticated)
                {
                    result.errorMessage = "You are not authenticated to call this method.";
                }
                else
                {
                    try
                    {
                        DataContext.KiggStory(storyId, CurrentUserId, _qualifyingKigg);
                        result.isSuccessful = true;
                    }
                    catch (InvalidOperationException e)
                    {
                        result.errorMessage = e.Message;
                    }
                }

                RenderView("Json", result);
            }
        }

        /// <summary>
        /// Post Comment for the specified story. The user must be authenticated prior calling this method.
        /// This is an Ajax Operation.
        /// </summary>
        /// <param name="storyId">The story id (Mandatory).</param>
        /// <param name="commentContent">Content of the comment (Mandatory).</param>
        public void Comment(int storyId, string commentContent)
        {
            using (CodeBenchmark.Start)
            {
                var result = new JsonResult();

                if (!IsUserAuthenticated)
                {
                    result.errorMessage = "You are not authenticated to call this method.";
                }
                else if (string.IsNullOrEmpty(commentContent))
                {
                    result.errorMessage = "Comment cannot be blank.";
                }
                else
                {
                    try
                    {
                        DataContext.PostComment(storyId, CurrentUserId, commentContent);
                        result.isSuccessful = true;
                    }
                    catch (InvalidOperationException e)
                    {
                        result.errorMessage = e.Message;
                    }
                }

                RenderView("Json", result);
            }
        }

        private T GetStoryListViewData<T>(int? page) where T : BaseStoryListData, new()
        {
            var viewData = GetViewData<T>();
            viewData.CurrentPage = page.HasValue ? page.Value : 1;
            viewData.StoryPerPage = _storyPerPage;

            return viewData;
        }

        private T GetViewData<T>() where T : BaseViewData, new()
        {
            var viewData = new T{
                                   IsAuthenticated = IsUserAuthenticated,
                                   UserName = CurrentUserName,
                                   Categories = DataContext.GetCategories(),
                                   Tags = DataContext.GetTags(_topTags)
                               };

            return viewData;
        }

        private static int CalculateStartIndex(int? page)
        {
            if (page.HasValue)
            {
                if (page.Value > 1)
                {
                    return ((page.Value - 1) * _storyPerPage);
                }
            }

            return 0;
        }

        private static bool IsValidUrl(string url)
        {
            return UrlExpression.IsMatch(url);
        }
    }
}