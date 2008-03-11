namespace Kigg.NUnitTest
{
    using System;
    using System.Web.Security;

    using NUnit.Framework;
    using Rhino.Mocks;

    using Kigg;

    [TestFixture]
    public class StoryControllerTest
    {
        private static readonly Guid DefaultUserID = Guid.NewGuid();

        private MockRepository mocks;
        private MembershipProvider userManager;
        private IDataContext dataContext;
        private StoryController controller;
        private MockViewEngine viewEngine;

        [SetUp]
        public void Init()
        {
            mocks = new MockRepository();
            userManager = mocks.MockMembershipProvider(true);
            dataContext = mocks.DynamicMock<IDataContext>();
            viewEngine = new MockViewEngine();
            controller = new StoryController(dataContext, userManager) {ViewEngine = viewEngine};
        }

        [Test]
        public void ShouldRenderForAllCategory()
        {
            using(mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new[] { new TagItem() });

                int total;
                Expect.Call(dataContext.GetPublishedStoriesForAllCategory(DefaultUserID, 0, 0, out total)).IgnoreArguments().OutRef(2000).Return(new[] { new StoryListItem(), new StoryListItem() });
            }

            using (mocks.Playback())
            {
                controller.Category(null, null);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Category");
                Assert.IsInstanceOfType(typeof(StoryListByCategoryData), viewEngine.ViewContext.ViewData);
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).Category, "All");
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).PageCount, 200);
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).CurrentPage, 1);
            }
        }

        [Test]
        public void ShouldRenderForSpecificCategory()
        {
            const int categoryId = -1;
            const string categoryName = "foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new[] { new TagItem() });

                Expect.Call(dataContext.GetCategoryByName(categoryName)).IgnoreArguments().Return(new Category { ID = categoryId, Name = categoryName });
                int total;
                Expect.Call(dataContext.GetPublishedStoriesForCategory(DefaultUserID, categoryId, 0, 0, out total)).IgnoreArguments().OutRef(99).Return(new [] { new StoryListItem() });
            }

            using (mocks.Playback())
            {
                controller.Category(categoryName, null);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Category");
                Assert.IsInstanceOfType(typeof(StoryListByCategoryData), viewEngine.ViewContext.ViewData);
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).Category, categoryName);
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).PageCount, 10);
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).CurrentPage, 1);
            }
        }

        [Test]
        public void ShouldRenderUpcoming()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new[] { new TagItem() });

                int total;
                Expect.Call(dataContext.GetUpcomingStories(DefaultUserID, 0, 0, out total)).IgnoreArguments().OutRef(500).Return(new [] { new StoryListItem() });
            }

            using (mocks.Playback())
            {
                controller.Upcoming(4);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Category");
                Assert.IsInstanceOfType(typeof(StoryListByCategoryData), viewEngine.ViewContext.ViewData);
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).Category, "Upcoming");
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).PageCount, 50);
                Assert.AreEqual(((StoryListByCategoryData)viewEngine.ViewContext.ViewData).CurrentPage, 4);
            }
        }

        [Test]
        public void ShouldRenderForSpecificTag()
        {
            const int tagId = -1;
            const string tagName = "foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new[] { new TagItem() });

                Expect.Call(dataContext.GetTagByName(tagName)).IgnoreArguments().Return(new Tag { ID = tagId, Name = tagName });

                int total;
                Expect.Call(dataContext.GetStoriesForTag(DefaultUserID, tagId, 0, 0, out total)).IgnoreArguments().Return(new [] { new StoryListItem() });
            }

            using (mocks.Playback())
            {
                controller.Tag(tagName, null);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Tag");
                Assert.IsInstanceOfType(typeof(StoryListByTagData), viewEngine.ViewContext.ViewData);
                Assert.AreEqual(((StoryListByTagData)viewEngine.ViewContext.ViewData).Tag, tagName);
                Assert.AreEqual(((StoryListByTagData)viewEngine.ViewContext.ViewData).PageCount, 1);
                Assert.AreEqual(((StoryListByTagData)viewEngine.ViewContext.ViewData).CurrentPage, 1);
            }
        }

        [Test]
        public void ShouldRenderAllCategoryForEmptyTag()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
                Expect.Call(delegate { controller.HttpContext.Response.Redirect(string.Empty); }).IgnoreArguments();
            }

            using (mocks.Playback())
            {
                controller.Tag(null, null);

                Assert.IsNull(viewEngine.ViewContext);
            }
        }

        [Test]
        public void ShouldRenderForSpecificPostedBy()
        {
            const string postedBy = "foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new[] { new TagItem() });

                int total;
                Expect.Call(dataContext.GetStoriesPostedByUser(DefaultUserID, DefaultUserID, 0, 0, out total)).IgnoreArguments().Return(new [] { new StoryListItem() });
            }

            using (mocks.Playback())
            {
                controller.PostedBy(postedBy, null);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "PostedBy");
                Assert.IsInstanceOfType(typeof(StoryListByUserData), viewEngine.ViewContext.ViewData);
                Assert.AreEqual(((StoryListByUserData)viewEngine.ViewContext.ViewData).PostedBy, postedBy);
                Assert.AreEqual(((StoryListByUserData)viewEngine.ViewContext.ViewData).PageCount, 1);
                Assert.AreEqual(((StoryListByUserData)viewEngine.ViewContext.ViewData).CurrentPage, 1);
            }
        }

        [Test]
        public void ShouldRenderAllCategoryForEmptyPostedBy()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
                Expect.Call(delegate { controller.HttpContext.Response.Redirect(string.Empty); }).IgnoreArguments();
            }

            using (mocks.Playback())
            {
                controller.PostedBy(null, null);

                Assert.IsNull(viewEngine.ViewContext);
            }
        }

        [Test]
        public void ShouldRenderSearchForSpecificSearchQuery()
        {
            const string searchQuery = "foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new[] { new TagItem() });

                int total;
                Expect.Call(dataContext.SearchStories(DefaultUserID, searchQuery, 0, 0, out total)).IgnoreArguments().Return(new [] { new StoryListItem() });
            }

            using (mocks.Playback())
            {
                controller.Search(searchQuery, null);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Search");
                Assert.IsInstanceOfType(typeof(StoryListBySearchData), viewEngine.ViewContext.ViewData);
                Assert.AreEqual(((StoryListBySearchData)viewEngine.ViewContext.ViewData).SearchQuery, searchQuery);
                Assert.AreEqual(((StoryListBySearchData)viewEngine.ViewContext.ViewData).PageCount, 1);
                Assert.AreEqual(((StoryListBySearchData)viewEngine.ViewContext.ViewData).CurrentPage, 1);
            }
        }

        [Test]
        public void ShouldRenderAllCategoryForEmptySearchQuery()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
                Expect.Call(delegate { controller.HttpContext.Response.Redirect(string.Empty); }).IgnoreArguments();
            }

            using (mocks.Playback())
            {
                controller.Search(null, null);

                Assert.IsNull(viewEngine.ViewContext);
            }
        }

        [Test]
        public void ShouldRenderDetailForSpecificStory()
        {
            const int storyId = -1;
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(dataContext.GetCategories()).IgnoreArguments().Return(new[] { new Category() });
                Expect.Call(dataContext.GetTags(50)).IgnoreArguments().Return(new[] { new TagItem() });

                Expect.Call(dataContext.GetStoryDetailById(DefaultUserID, storyId)).IgnoreArguments().Return(new StoryDetailItem { ID = storyId });
            }

            using (mocks.Playback())
            {
                controller.Detail(storyId);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Detail");
                Assert.IsInstanceOfType(typeof(StoryDetailData), viewEngine.ViewContext.ViewData);
                Assert.AreEqual(((StoryDetailData)viewEngine.ViewContext.ViewData).Story.ID, storyId);
            }
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

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);

                Expect.Call(dataContext.SubmitStory(url, title, categoryId, description, tags, DefaultUserID)).IgnoreArguments().Return(id);
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsTrue(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.IsNull(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage);
            }
        }

        [Test]
        public void ShouldNotSubmitForUnauthenticatedUser()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, false);
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "You are not authenticated to call this method.");
            }
        }

        [Test]
        public void ShouldNotSubmitForEmptyUrl()
        {
            const string url = "";
            const string title = "";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Story url cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotSubmitForInvalidUrl()
        {
            const string url = "foo";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Invalid url.");
            }
        }

        [Test]
        public void ShouldNotSubmitForEmptyTitle()
        {
            const string url = "http://www.foo.com";
            const string title = "";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Story title cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotSubmitForEmptyDescription()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "";
            const int categoryId = -1;
            const string tags = "Foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Story description cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotSubmitForInvalidCategory()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);

                Expect.Call(dataContext.SubmitStory(url, title, categoryId, description, tags, DefaultUserID)).IgnoreArguments().Throw(new InvalidOperationException("Specified category does not exist."));
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Specified category does not exist.");
            }
        }

        [Test]
        public void ShouldNotSubmitForDuplicateUrl()
        {
            const string url = "http://www.foo.com";
            const string title = "Foo";
            const string description = "Foo";
            const int categoryId = -1;
            const string tags = "Foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);

                Expect.Call(dataContext.SubmitStory(url, title, categoryId, description, tags, DefaultUserID)).IgnoreArguments().Throw(new InvalidOperationException("Specified story already exists."));
            }

            using (mocks.Playback())
            {
                controller.Submit(url, title, categoryId, description, tags);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Specified story already exists.");
            }
        }

        [Test]
        public void ShouldKiggForAuthenticatedUser()
        {
            const int storyId = -1;

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);

                Expect.Call(delegate { dataContext.KiggStory(storyId, DefaultUserID, 0); }).IgnoreArguments();
            }

            using (mocks.Playback())
            {
                controller.Kigg(storyId);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsTrue(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.IsNull(((JsonResult) viewEngine.ViewContext.ViewData).errorMessage);
            }
        }

        [Test]
        public void ShouldNotKiggForUnauthenticatedUser()
        {
            const int storyId = -1;

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, false);
            }

            using (mocks.Playback())
            {
                controller.Kigg(storyId);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "You are not authenticated to call this method.");
            }
        }

        [Test]
        public void ShouldNotKiggForInvalidStory()
        {
            const int storyId = -1;

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);

                Expect.Call(delegate { dataContext.KiggStory(storyId, DefaultUserID, 0); }).IgnoreArguments().Throw(new InvalidOperationException("Specified story does not exist."));
            }

            using (mocks.Playback())
            {
                controller.Kigg(storyId);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Specified story does not exist.");
            }
        }

        [Test]
        public void ShouldCommentForAuthenticatedUser()
        {
            const int storyId = -1;
            const string content = "foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);

                Expect.Call(dataContext.PostComment(storyId, DefaultUserID, content)).IgnoreArguments().Return(storyId);
            }

            using (mocks.Playback())
            {
                controller.Comment(storyId, content);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsTrue(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.IsNull(((JsonResult) viewEngine.ViewContext.ViewData).errorMessage);
            }
        }

        [Test]
        public void ShouldNotCommentForUnauthenticatedUser()
        {
            const int storyId = -1;
            const string content = "foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, false);
            }

            using (mocks.Playback())
            {
                controller.Comment(storyId, content);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "You are not authenticated to call this method.");
            }
        }

        [Test]
        public void ShouldNotCommentForEmptyContent()
        {
            const int storyId = -1;
            const string content = "";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);
            }

            using (mocks.Playback())
            {
                controller.Comment(storyId, content);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Comment cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotCommentForInvalidStory()
        {
            const int storyId = -1;
            const string content = "foo";

            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);

                Expect.Call(dataContext.PostComment(storyId, DefaultUserID, content)).IgnoreArguments().Throw(new InvalidOperationException("Specified story does not exist."));
            }

            using (mocks.Playback())
            {
                controller.Comment(storyId, content);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Specified story does not exist.");
            }
        }
    }
}