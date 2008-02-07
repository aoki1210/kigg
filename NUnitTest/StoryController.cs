namespace Kigg.NUnitTest
{
    using System;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Security;
    using System.Web.Mvc;

    using NUnit.Framework;
    using Rhino.Mocks;

    using Kigg;

    [TestFixture]
    public class StoryControllerTest
    {
        private const string DefaultUserName = "foobar";
        private static readonly Guid DefaultUserID = Guid.NewGuid();

        private MockRepository mocks = null;

        [SetUp]
        public void Init()
        {
            mocks = new MockRepository();
        }

        [Test]
        public void ShouldRenderForAllCategory()
        {
            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, true);
                MembershipProvider userManager = GetMembershipProvider(mocks);

                controller = new StoryControllerForTest(dataContext, userManager);

                int total;
                Expect.Call(dataContext.GetPublishedStoriesForAllCategory(DefaultUserID, 0, 0, out total)).IgnoreArguments().OutRef(2000).Return(new StoryListItem[] { new StoryListItem(), new StoryListItem() });

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Category(null, null);
            }

            Assert.AreEqual(controller.SelectedView, "Category");
            Assert.IsInstanceOfType(typeof(StoryListByCategoryData), controller.SelectedViewData);
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).Category, "All");
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).PageCount, 200);
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).CurrentPage, 1);
        }

        [Test]
        public void ShouldRenderForSpecificCategory()
        {
            const int categoryId = -1;
            const string categoryName = "foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, true);
                MembershipProvider userManager = GetMembershipProvider(mocks);

                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(dataContext.GetCategoryByName(categoryName)).IgnoreArguments().Return(new Category { ID = categoryId, Name = categoryName });

                int total = 0;
                Expect.Call(dataContext.GetPublishedStoriesForCategory(DefaultUserID, categoryId, 0, 0, out total)).IgnoreArguments().OutRef(99).Return(new StoryListItem[] { new StoryListItem() });

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Category(categoryName, null);
            }

            Assert.AreEqual(controller.SelectedView, "Category");
            Assert.IsInstanceOfType(typeof(StoryListByCategoryData), controller.SelectedViewData);
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).Category, categoryName);
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).PageCount, 10);
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).CurrentPage, 1);
        }

        [Test]
        public void ShouldRenderUpcoming()
        {
            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, true);
                MembershipProvider userManager = GetMembershipProvider(mocks);

                controller = new StoryControllerForTest(dataContext, userManager);

                int total;
                Expect.Call(dataContext.GetUpcomingStories(DefaultUserID, 0, 0, out total)).IgnoreArguments().OutRef(500).Return(new StoryListItem[] { new StoryListItem() });

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Upcoming(4);
            }

            Assert.AreEqual(controller.SelectedView, "Category");
            Assert.IsInstanceOfType(typeof(StoryListByCategoryData), controller.SelectedViewData);
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).Category, "Upcoming");
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).PageCount, 50);
            Assert.AreEqual(((StoryListByCategoryData)controller.SelectedViewData).CurrentPage, 4);
        }

        [Test]
        public void ShouldRenderForSpecificTag()
        {
            const int tagId = -1;
            const string tagName = "foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, true);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(dataContext.GetTagByName(tagName)).IgnoreArguments().Return(new Tag { ID = tagId, Name = tagName });

                int total = 0;
                Expect.Call(dataContext.GetStoriesForTag(DefaultUserID, tagId, 0, 0, out total)).IgnoreArguments().Return(new StoryListItem[] { new StoryListItem() });

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Tag(tagName, null);
            }

            Assert.AreEqual(controller.SelectedView, "Tag");
            Assert.IsInstanceOfType(typeof(StoryListByTagData), controller.SelectedViewData);
            Assert.AreEqual(((StoryListByTagData)controller.SelectedViewData).Tag, tagName);
            Assert.AreEqual(((StoryListByTagData)controller.SelectedViewData).PageCount, 1);
            Assert.AreEqual(((StoryListByTagData)controller.SelectedViewData).CurrentPage, 1);
        }

        [Test]
        public void ShouldRenderAllCategoryForEmptyTag()
        {
            IDataContext dataContext = GetDataContext(mocks, false);
            MembershipProvider userManager = GetMembershipProvider(mocks);
            StoryControllerForTest controller = new StoryControllerForTest(dataContext, userManager);

            controller.Tag(null, null);

            Assert.AreEqual(controller.RedirectedAction, "Category");
        }

        [Test]
        public void ShouldRenderForSpecificPostedBy()
        {
            const string postedBy = "foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, true);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                int total;
                Expect.Call(dataContext.GetStoriesPostedByUser(DefaultUserID, DefaultUserID, 0, 0, out total)).IgnoreArguments().Return(new StoryListItem[] { new StoryListItem() });

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.PostedBy(postedBy, null);
            }

            Assert.AreEqual(controller.SelectedView, "PostedBy");
            Assert.IsInstanceOfType(typeof(StoryListByUserData), controller.SelectedViewData);
            Assert.AreEqual(((StoryListByUserData)controller.SelectedViewData).PostedBy, postedBy);
            Assert.AreEqual(((StoryListByUserData)controller.SelectedViewData).PageCount, 1);
            Assert.AreEqual(((StoryListByUserData)controller.SelectedViewData).CurrentPage, 1);
        }

        [Test]
        public void ShouldRenderAllCategoryForEmptyPostedBy()
        {
            IDataContext dataContext = GetDataContext(mocks, false);
            MembershipProvider userManager = GetMembershipProvider(mocks);
            StoryControllerForTest controller = new StoryControllerForTest(dataContext, userManager);

            controller.PostedBy(null, null);

            Assert.AreEqual(controller.RedirectedAction, "Category");
        }

        [Test]
        public void ShouldRenderSearchForSpecificSearchQuery()
        {
            const string searchQuery = "foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, true);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                int total;
                Expect.Call(dataContext.SearchStories(DefaultUserID, searchQuery, 0, 0, out total)).IgnoreArguments().Return(new StoryListItem[] { new StoryListItem() });

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Search(searchQuery, null);
            }

            Assert.AreEqual(controller.SelectedView, "Search");
            Assert.IsInstanceOfType(typeof(StoryListBySearchData), controller.SelectedViewData);
            Assert.AreEqual(((StoryListBySearchData)controller.SelectedViewData).SearchQuery, searchQuery);
            Assert.AreEqual(((StoryListBySearchData)controller.SelectedViewData).PageCount, 1);
            Assert.AreEqual(((StoryListBySearchData)controller.SelectedViewData).CurrentPage, 1);
        }

        [Test]
        public void ShouldRenderAllCategoryForEmptySearchQuery()
        {
            IDataContext dataContext = GetDataContext(mocks, false);
            MembershipProvider userManager = GetMembershipProvider(mocks);
            StoryControllerForTest controller = new StoryControllerForTest(dataContext, userManager);

            controller.Search(null, null);

            Assert.AreEqual(controller.RedirectedAction, "Category");
        }

        [Test]
        public void ShouldRenderDetailForSpecificStory()
        {
            const int storyId = -1;

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, true);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(dataContext.GetStoryDetailById(DefaultUserID, storyId)).IgnoreArguments().Return(new StoryDetailItem { ID = storyId });

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Detail(storyId);
            }

            Assert.AreEqual(controller.SelectedView, "Detail");
            Assert.IsInstanceOfType(typeof(StoryDetailData), controller.SelectedViewData);
            Assert.AreEqual(((StoryDetailData)controller.SelectedViewData).Story.ID, storyId);
        }

        [Test]
        public void ShouldSubmitForAuthenticatedUser()
        {
            const int id = -1;
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(dataContext.SubmitStory(url, title, categoryId, description, tags, DefaultUserID)).IgnoreArguments().Return(id);

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsTrue(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.IsNull(((JsonResult)controller.SelectedViewData).errorMessage);
        }

        [Test]
        public void ShouldNotSubmitForUnauthenticatedUser()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, false);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "You are not authenticated to call this method.");
        }

        [Test]
        public void ShouldNotSubmitForEmptyUrl()
        {
            const string url = "";
            const string title = "";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Story url cannot be blank.");
        }

        [Test]
        public void ShouldNotSubmitForInvalidUrl()
        {
            const string url = "foo";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Invalid url.");
        }

        [Test]
        public void ShouldNotSubmitForEmptyTitle()
        {
            const string url = "http://www.foo.com";
            const string title = "";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Story title cannot be blank.");
        }

        [Test]
        public void ShouldNotSubmitForEmptyDescription()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Story description cannot be blank.");
        }

        [Test]
        public void ShouldNotSubmitForInvalidCategory()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(dataContext.SubmitStory(url, title, categoryId, description, tags, DefaultUserID)).IgnoreArguments().Throw(new InvalidOperationException("Specified category does not exist."));

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Specified category does not exist.");
        }

        [Test]
        public void ShouldNotSubmitForDuplicateUrl()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(dataContext.SubmitStory(url, title, categoryId, description, tags, DefaultUserID)).IgnoreArguments().Throw(new InvalidOperationException("Specified story already exists."));

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Submit(url, title, categoryId, description, tags);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Specified story already exists.");
        }

        [Test]
        public void ShouldKiggForAuthenticatedUser()
        {
            const int storyId = -1;

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(delegate { dataContext.KiggStory(storyId, DefaultUserID, 0); }).IgnoreArguments();

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Kigg(storyId);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsTrue(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.IsNull(((JsonResult)controller.SelectedViewData).errorMessage);
        }

        [Test]
        public void ShouldNotKiggForUnauthenticatedUser()
        {
            const int storyId = -1;

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, false);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Kigg(storyId);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "You are not authenticated to call this method.");
        }

        [Test]
        public void ShouldNotKiggForInvalidStory()
        {
            const int storyId = -1;

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(delegate { dataContext.KiggStory(storyId, DefaultUserID, 0); }).IgnoreArguments().Throw(new InvalidOperationException("Specified story does not exist."));

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Kigg(storyId);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Specified story does not exist.");
        }

        [Test]
        public void ShouldCommentForAuthenticatedUser()
        {
            const int storyId = -1;
            const string content = "foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(dataContext.PostComment(storyId, DefaultUserID, content)).IgnoreArguments().Return(-1);

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Comment(storyId, content);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsTrue(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.IsNull(((JsonResult)controller.SelectedViewData).errorMessage);
        }

        [Test]
        public void ShouldNotCommentForUnauthenticatedUser()
        {
            const int storyId = -1;
            const string content = "foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, false);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Comment(storyId, content);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "You are not authenticated to call this method.");
        }

        [Test]
        public void ShouldNotCommentForEmptyContent()
        {
            const int storyId = -1;
            const string content = "";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Comment(storyId, content);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Comment cannot be blank.");
        }

        [Test]
        public void ShouldNotCommentForInvalidStory()
        {
            const int storyId = -1;
            const string content = "foo";

            StoryControllerForTest controller;
            ControllerContext controllerContext;

            using (mocks.Record())
            {
                IDataContext dataContext = GetDataContext(mocks, false);
                MembershipProvider userManager = GetMembershipProvider(mocks);
                controller = new StoryControllerForTest(dataContext, userManager);

                Expect.Call(delegate { dataContext.PostComment(storyId, DefaultUserID, content); }).IgnoreArguments().Throw(new InvalidOperationException("Specified story does not exist."));

                IHttpContext httpContext = GetHttpContext(mocks, true);
                controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            }

            using (mocks.Playback())
            {
                controller.ControllerContext = controllerContext;
                controller.Comment(storyId, content);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(typeof(JsonResult), controller.SelectedViewData);
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Specified story does not exist.");
        }

        private static IDataContext GetDataContext(MockRepository mocks, bool populateCategoryAndTag)
        {
            IDataContext dataContext = mocks.DynamicMock<IDataContext>();

            if (populateCategoryAndTag)
            {
                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new Category[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new TagItem[] { new TagItem() });
            }

            return dataContext;
        }

        private static IHttpContext GetHttpContext(MockRepository mocks, bool authenticated)
        {
            IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
            IHttpRequest httpRequest = mocks.DynamicMock<IHttpRequest>();
            IHttpResponse httpResponse = mocks.DynamicMock<IHttpResponse>();
            IHttpSessionState httpSession = mocks.DynamicMock<IHttpSessionState>();
            IHttpServerUtility httpServer = mocks.DynamicMock<IHttpServerUtility>();

            IPrincipal principal = mocks.DynamicMock<IPrincipal>();
            IIdentity identity = mocks.DynamicMock<IIdentity>();

            SetupResult.For(httpContext.Request).Return(httpRequest);
            SetupResult.For(httpContext.Response).Return(httpResponse);
            SetupResult.For(httpContext.Session).Return(httpSession);
            SetupResult.For(httpContext.Server).Return(httpServer);

            SetupResult.For(identity.IsAuthenticated).Return(authenticated);
            SetupResult.For(identity.Name).Return(DefaultUserName);
            SetupResult.For(principal.Identity).Return(identity);
            SetupResult.For(httpContext.User).Return(principal);

            return httpContext;
        }

        private static MembershipProvider GetMembershipProvider(MockRepository mocks)
        {
            MembershipUser user = mocks.Stub<MembershipUser>();

            SetupResult.For(user.ProviderUserKey).Return(DefaultUserID);
            SetupResult.For(user.UserName).Return(DefaultUserName);

            MembershipProvider userManager = mocks.PartialMock<MembershipProvider>();

            SetupResult.For(userManager.GetUser(DefaultUserName, true)).IgnoreArguments().Return(user);

            return userManager;
        }

        private class StoryControllerForTest : StoryController
        {
            public string SelectedView
            {
                get;
                private set;
            }

            public object SelectedViewData
            {
                get;
                private set;
            }

            public string RedirectedAction
            {
                get;
                private set;
            }

            public StoryControllerForTest(IDataContext dataContext, MembershipProvider userManager): base(dataContext, userManager)
            {
            }

            protected override void RenderView(string viewName, string masterName, object viewData)
            {
                SelectedView = viewName;
                SelectedViewData = viewData;
            }

            protected override void RedirectToAction(object values)
            {
                RedirectedAction = (string)values.GetType().GetProperty("Action").GetValue(values, null);
            }
        }
    }
}