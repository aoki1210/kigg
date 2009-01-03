namespace Kigg.VSTest
{
    using System;
    using System.Linq;
    using System.Transactions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Kigg;

    [TestClass]
    public class KiggDataContextTest
    {
        private const int DefaultPageSize = 10;

        private const string CategoryName = "Science";
        private const string TagName = "Microsoft";
        private static readonly Guid DefaultUserID = Guid.NewGuid();

        private KiggDataContext dataContext;

        [TestInitialize]
        public void Init()
        {
            dataContext = new KiggDataContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }

        [TestMethod]
        public void VerifyGetCategories()
        {
            var categories = dataContext.GetCategories();

            Assert.AreEqual(categories.Length, dataContext.Categories.Count());
        }

        [TestMethod]
        public void VerifyGetCategoryByName()
        {
            var category = dataContext.GetCategoryByName(CategoryName);

            if (category != null)
            {
                Assert.AreEqual(category.Name, CategoryName);
            }
        }

        [TestMethod]
        public void VerifyGetTagByName()
        {
            var tag = dataContext.GetTagByName(TagName);

            if (tag != null)
            {
                Assert.AreEqual(tag.Name, TagName);
            }
        }

        [TestMethod]
        public void VerifyGetTags()
        {
            const int tagCount = 10;

            var tags = dataContext.GetTags(tagCount);

            Assert.AreEqual(tags.Length, tagCount);
        }

        [TestMethod]
        public void VerifyGetStoryDetailById()
        {
            var story = dataContext.Stories.FirstOrDefault();
            
            if (story != null)
            {
                var storyItem = dataContext.GetStoryDetailById(DefaultUserID, story.ID);

                Assert.IsNotNull(storyItem);
                Assert.AreEqual(story.ID, storyItem.ID);
                Assert.AreEqual(story.Title, storyItem.Title);
                Assert.AreEqual(story.Url, storyItem.Url);
                Assert.AreEqual(story.Description, storyItem.Description);
                Assert.AreEqual(story.PostedOn, storyItem.PostedOn);
                Assert.AreEqual(story.PublishedOn, storyItem.PublishedOn);
                Assert.AreEqual(story.Category.Name, storyItem.Category);
                Assert.AreEqual(story.User.UserName, storyItem.PostedBy.Name);
            }
        }

        [TestMethod]
        public void VerifyGetPublishedStoriesForAllCategory()
        {
            int total;
            var stories = dataContext.GetPublishedStoriesForAllCategory(DefaultUserID, 0, DefaultPageSize, out total);

            if (stories.Length == 0)
            {
                Assert.AreEqual(total, 0);
            }
            else
            {
                Assert.IsTrue(stories.Length <= DefaultPageSize);
                Assert.IsNotNull(stories[0].PublishedOn);
                Assert.AreEqual(total, dataContext.Stories.Where(s => s.PublishedOn != null).Count());
            }
        }

        [TestMethod]
        public void VerifyGetPublishedStoriesForCategory()
        {
            var category = dataContext.GetCategoryByName(CategoryName);

            if (category != null)
            {
                int total;
                var stories = dataContext.GetPublishedStoriesForCategory(DefaultUserID, category.ID, 0, DefaultPageSize, out total);

                if (stories.Length == 0)
                {
                    Assert.AreEqual(total, 0);
                }
                else
                {
                    Assert.IsTrue(stories.Length <= DefaultPageSize);

                    for (var i = 0; i < stories.Length; i++ )
                    {
                        Assert.IsNotNull(stories[i].PublishedOn);
                        Assert.AreEqual(stories[i].Category, category.Name);
                    }

                    Assert.AreEqual(total, dataContext.Stories.Where(s => s.PublishedOn != null && s.CategoryID == category.ID).Count());
                }
            }
        }

        [TestMethod]
        public void VerifyGetStoriesForTag()
        {
            var tag = dataContext.Tags.SingleOrDefault(t => t.Name == TagName);

            if (tag != null)
            {
                int total;
                var stories = dataContext.GetStoriesForTag(DefaultUserID, tag.ID, 0, DefaultPageSize, out total);

                if (stories.Length == 0)
                {
                    Assert.AreEqual(total, 0);
                }
                else
                {
                    Assert.IsTrue(stories.Length <= DefaultPageSize);

                    for (var i = 0; i < stories.Length; i++)
                    {
                        Assert.IsTrue(stories[i].Tags.Where(t => tag.Name == t).Count() == 1);
                    }

                    Assert.AreEqual(total, dataContext.Stories.Where(s => s.StoryTags.Count(st => st.TagID == tag.ID) > 0).Count());
                }
            }
        }

        [TestMethod]
        public void VerifyGetUpcomingStories()
        {
            int total;
            var stories = dataContext.GetUpcomingStories(DefaultUserID, 0, DefaultPageSize, out total);

            if (stories.Length == 0)
            {
                Assert.AreEqual(total, 0);
            }
            else
            {
                Assert.IsTrue(stories.Length <= DefaultPageSize);

                for (var i = 0; i < stories.Length; i++)
                {
                    Assert.IsNull(stories[i].PublishedOn);
                }

                Assert.AreEqual(total, dataContext.Stories.Where(s => s.PublishedOn == null).Count());
            }
        }

        [TestMethod]
        public void VerifyGetStoriesPostedByUser()
        {
            var tempStory = dataContext.Stories.FirstOrDefault();

            if (tempStory != null)
            {
                int total;

                var stories = dataContext.GetStoriesPostedByUser(DefaultUserID, tempStory.PostedBy, 0, DefaultPageSize, out total);

                Assert.IsTrue(stories.Length <= DefaultPageSize);

                for (var i = 0; i < stories.Length; i++)
                {
                    Assert.AreEqual(stories[i].PostedBy.Name, tempStory.User.UserName);
                }

                Assert.AreEqual(total, dataContext.Stories.Where(s => s.PostedBy == tempStory.PostedBy).Count());
            }
        }

        [TestMethod]
        public void VerifySearchStories()
        {
            int total;
            const string query = "XBOX";

            var stories = dataContext.SearchStories(DefaultUserID, query, 0, DefaultPageSize, out total);

            if (stories.Length == 0)
            {
                Assert.AreEqual(total, 0);
            }
            else
            {
                Assert.IsTrue(stories.Length <= DefaultPageSize);

                for (var i = 0; i < stories.Length; i++)
                {
                    Assert.IsTrue(stories[i].Title.ToUpperInvariant().Contains(query) || stories[i].Description.ToUpperInvariant().Contains(query) || stories[i].Category.ToUpperInvariant().Contains(query) || (stories[i].Tags.Where(t => t.ToUpperInvariant().Contains(query)).Count() == 1));
                }

                Assert.AreEqual(total, dataContext.Stories.Where(s => (s.Title.Contains(query)) || (s.Description.Contains(query)) || (s.Category.Name.Contains(query)) || (s.StoryTags.Count(st => st.Tag.Name.Contains(query)) > 0)).Count());
            }
        }

        [TestMethod]
        public void VerifySubmitStory()
        {
            const string url = "http://www.foobar.com";
            const string title = "Foo Bar";
            const string description = "Foo Bar";
            const string tags = "foo, bar";

            var tempStory = dataContext.Stories.FirstOrDefault();

            if (tempStory != null)
            {
                using (new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    var id = dataContext.SubmitStory(url, title, 1, description, tags, tempStory.PostedBy);

                    var story = dataContext.Stories.First(s => s.ID == id);

                    Assert.IsNotNull(story);
                    Assert.AreEqual(story.ID, id);
                    Assert.AreEqual(story.Url, url);
                    Assert.AreEqual(story.Title, title);
                    Assert.AreEqual(story.Description, description);
                    Assert.AreEqual(story.PostedBy, tempStory.PostedBy);
                }
            }
        }

        [TestMethod]
        public void VerifyKiggStory()
        {
            var tempStory1 = dataContext.Stories.FirstOrDefault();

            if (tempStory1 != null)
            {
                var tempStory2 = dataContext.Stories.FirstOrDefault(s => s.PostedBy != tempStory1.PostedBy);

                if (tempStory2 != null)
                {
                    using (new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        dataContext.KiggStory(tempStory2.ID, tempStory1.PostedBy, 3);

                        Assert.IsNotNull(dataContext.Votes.SingleOrDefault(v => v.StoryID == tempStory2.ID && v.UserID == tempStory1.PostedBy));
                    }
                }
            }
        }

        [TestMethod]
        public void VerifyPostComment()
        {
            const string commentContent = "Comment Foo Bar.";

            var tempStory = dataContext.Stories.FirstOrDefault();

            if (tempStory != null)
            {
                using (new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    var id = dataContext.PostComment(tempStory.ID, tempStory.PostedBy, commentContent);

                    var comment = dataContext.Comments.Single(c => c.ID == id);

                    Assert.IsNotNull(comment);
                    Assert.AreEqual(comment.ID, id);
                    Assert.AreEqual(comment.Content, commentContent);
                    Assert.AreEqual(comment.StoryID, tempStory.ID);
                    Assert.AreEqual(comment.PostedBy, tempStory.PostedBy);
                }
            }
        }
    }
}