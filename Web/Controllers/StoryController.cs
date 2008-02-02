namespace Kigg
{
    using System;
    using System.Text.RegularExpressions;
    using System.Configuration;
    using System.Collections;
    using System.Web.Security;
    using System.Web.Mvc;

    public class StoryController : BaseController
    {
        private static readonly Regex UrlExpression = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly int _storyPerPage = 10;
        private static readonly int _topTags = 50;
        private static readonly int _qualifyingKigg = 3;

        private IDataContext _dataContext;

        private IDataContext DataContext
        {
            [System.Diagnostics.DebuggerStepThrough()]
            get
            {
                return _dataContext;
            }
        }

        static StoryController()
        {
            Hashtable settings = ConfigurationManager.GetSection("storySettings") as Hashtable;

            if (settings != null)
            {
                _storyPerPage = Convert.ToInt32(settings["storyPerPage"]);
                _topTags = Convert.ToInt32(settings["topTags"]);
                _qualifyingKigg = Convert.ToInt32(settings["qualifyingKigg"]);
            }
        }

        public StoryController(IDataContext dataContext, MembershipProvider userManager):base(userManager)
        {
            _dataContext = dataContext;
        }

        public StoryController():this(new KiggDataContext(), Membership.Provider)
        {
        }

        [ControllerAction]
        public void Category(string name, int? page)
        {
            using (new CodeBenchmark())
            {
                StoryListByCategoryData viewData = GetStoryListViewData<StoryListByCategoryData>(page);

                int total = 0;

                if (string.IsNullOrEmpty(name))
                {
                    viewData.Stories = DataContext.GetPublishedStoriesForAllCategory(CurrentUserId, CalculateStartIndex(page), _storyPerPage, out total);
                    name = "All";
                }
                else
                {
                    Category category = DataContext.GetCategoryByName(name);

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

        [ControllerAction]
        public void Upcoming(int? page)
        {
            using (new CodeBenchmark())
            {
                StoryListByCategoryData viewData = GetStoryListViewData<StoryListByCategoryData>(page);
                int total = 0;

                viewData.Stories = DataContext.GetUpcomingStories(CurrentUserId, CalculateStartIndex(page), _storyPerPage, out total);
                viewData.StoryCount = total;
                viewData.Category = "Upcoming";

                RenderView("Category", viewData);
            }
        }

        [ControllerAction]
        public void Tag(string name, int? page)
        {
            if (string.IsNullOrEmpty(name))
            {
                RedirectToAction("Category");
                return;
            }

            using (new CodeBenchmark())
            {
                StoryListByTagData viewData = GetStoryListViewData<StoryListByTagData>(page);

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

        [ControllerAction]
        public void PostedBy(string name, int? page)
        {
            if (string.IsNullOrEmpty(name))
            {
                RedirectToAction("Category");
                return;
            }

            using (new CodeBenchmark())
            {
                StoryListByUserData viewData = GetStoryListViewData<StoryListByUserData>(page);
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

        [ControllerAction]
        public void Search(string q, int? page)
        {
            if (string.IsNullOrEmpty(q))
            {
                RedirectToAction("Category");
                return;
            }

            using (new CodeBenchmark())
            {
                StoryListBySearchData viewData = GetStoryListViewData<StoryListBySearchData>(page);
                int total = 0;

                viewData.Stories = DataContext.SearchStories(CurrentUserId, q, CalculateStartIndex(page), _storyPerPage, out total);
                viewData.StoryCount = total;
                viewData.SearchQuery = q;

                RenderView("Search", viewData);
            }
        }

        [ControllerAction]
        public void Detail(int id)
        {
            using (new CodeBenchmark())
            {
                StoryDetailData viewData = GetViewData<StoryDetailData>();

                viewData.Story = DataContext.GetStoryDetailById(CurrentUserId, id);

                RenderView("Detail", viewData);
            }
        }

        [ControllerAction]
        public void Create(string storyUrl, string storyTitle, int storyCategoryId, string storyDescription, string storyTags)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (!IsUserAuthenticated)
                {
                    result.errorMessage = "You are not authenticated to call this method.";
                }
                else
                {
                    if (string.IsNullOrEmpty(storyUrl))
                    {
                        result.errorMessage = "Story url cannot be blank.";
                    }
                    else
                    {
                        if (!IsValidUrl(storyUrl))
                        {
                            result.errorMessage = "Invalid url.";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(storyTitle))
                            {
                                result.errorMessage = "Story title cannot be blank.";
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(storyDescription))
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
                            }
                        }
                    }
                }

                RenderView("Json", result);
            }
        }

        [ControllerAction]
        public void Kigg(int storyId)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

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

        [ControllerAction]
        public void Comment(int storyId, string commentContent)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (!IsUserAuthenticated)
                {
                    result.errorMessage = "You are not authenticated to call this method.";
                }
                else
                {
                    if (string.IsNullOrEmpty(commentContent))
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
                }

                RenderView("Json", result);
            }
        }

        private T GetStoryListViewData<T>(int? page) where T : BaseStoryListData, new()
        {
            T viewData = GetViewData<T>();
            viewData.CurrentPage = page.HasValue ? page.Value : 1;
            viewData.StoryPerPage = _storyPerPage;

            return viewData;
        }

        private T GetViewData<T>() where T : BaseViewData, new()
        {
            T viewData = new T();

            viewData.IsAuthenticated = IsUserAuthenticated;
            viewData.UserName = CurrentUserName;
            viewData.Categories = DataContext.GetCategories();
            viewData.Tags = DataContext.GetTags(_topTags);

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