namespace Kigg.NUnitTest
{
    using System;
    using System.Linq;
    using System.Data.Linq;
    using System.Transactions;

    using NUnit.Framework;

    using Kigg;

    [TestFixture]
    public class KiggDataContextTest
    {
        private const int DefaultPageSize = 10;

        private const string CategoryName = "Science";
        private const string TagName = "Microsoft";
        private static readonly Guid DefaultUserID = Guid.NewGuid();

        KiggDataContext dataContext = new KiggDataContext();

        [SetUp]
        public void Init()
        {
            dataContext = new KiggDataContext();
        }

        [TearDown]
        public void Cleanup()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
            }
        }

        [Test]
        public void VerifyGetCategories()
        {
            Category[] categories = dataContext.GetCategories();

            Assert.AreEqual(categories.Length, dataContext.Categories.Count());
        }

        [Test]
        public void VerifyGetCategoryByName()
        {
            Category category = dataContext.GetCategoryByName(CategoryName);

            if (category != null)
            {
                Assert.AreEqual(category.Name, CategoryName);
            }
        }

        [Test]
        public void VerifyGetTagByName()
        {
            Tag tag = dataContext.GetTagByName(TagName);

            if (tag != null)
            {
                Assert.AreEqual(tag.Name, TagName);
            }
        }

        [Test]
        public void VerifyGetTags()
        {
            const int tagCount = 10;

            TagItem[] tags = dataContext.GetTags(tagCount);

            Assert.AreEqual(tags.Length, tagCount);
        }

        [Test]
        public void VerifyGetStoryDetailById()
        {
            Story story = dataContext.Stories.FirstOrDefault();

            if (story != null)
            {
                StoryDetailItem storyItem = dataContext.GetStoryDetailById(DefaultUserID, story.ID);

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

        [Test]
        public void VerifyGetPublishedStoriesForAllCategory()
        {
            int total;
            StoryListItem[] stories = dataContext.GetPublishedStoriesForAllCategory(DefaultUserID, 0, DefaultPageSize, out total);

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

        [Test]
        public void VerifyGetPublishedStoriesForCategory()
        {
            Category category = dataContext.GetCategoryByName(CategoryName);

            if (category != null)
            {
                int total;
                StoryListItem[] stories = dataContext.GetPublishedStoriesForCategory(DefaultUserID, category.ID, 0, DefaultPageSize, out total);

                if (stories.Length == 0)
                {
                    Assert.AreEqual(total, 0);
                }
                else
                {
                    Assert.IsTrue(stories.Length <= DefaultPageSize);

                    for (int i = 0; i < stories.Length; i++)
                    {
                        Assert.IsNotNull(stories[i].PublishedOn);
                        Assert.AreEqual(stories[i].Category, category.Name);
                    }

                    Assert.AreEqual(total, dataContext.Stories.Where(s => s.PublishedOn != null && s.CategoryID == category.ID).Count());
                }
            }
        }

        [Test]
        public void VerifyGetStoriesForTag()
        {
            Tag tag = dataContext.Tags.SingleOrDefault(t => t.Name == TagName);

            if (tag != null)
            {
                int total;
                StoryListItem[] stories = dataContext.GetStoriesForTag(DefaultUserID, tag.ID, 0, DefaultPageSize, out total);

                if (stories.Length == 0)
                {
                    Assert.AreEqual(total, 0);
                }
                else
                {
                    Assert.IsTrue(stories.Length <= DefaultPageSize);

                    for (int i = 0; i < stories.Length; i++)
                    {
                        Assert.IsTrue(stories[i].Tags.Where(t => tag.Name == t).Count() == 1);
                    }

                    Assert.AreEqual(total, dataContext.Stories.Where(s => s.StoryTags.Count(st => st.TagID == tag.ID) > 0).Count());
                }
            }
        }

        [Test]
        public void VerifyGetUpcomingStories()
        {
            int total;
            StoryListItem[] stories = dataContext.GetUpcomingStories(DefaultUserID, 0, DefaultPageSize, out total);

            if (stories.Length == 0)
            {
                Assert.AreEqual(total, 0);
            }
            else
            {
                Assert.IsTrue(stories.Length <= DefaultPageSize);

                for (int i = 0; i < stories.Length; i++)
                {
                    Assert.IsNull(stories[i].PublishedOn);
                }

                Assert.AreEqual(total, dataContext.Stories.Where(s => s.PublishedOn == null).Count());
            }
        }

        [Test]
        public void VerifyGetStoriesPostedByUser()
        {
            Story tempStory = dataContext.Stories.FirstOrDefault();

            if (tempStory != null)
            {
                int total;

                StoryListItem[] stories = dataContext.GetStoriesPostedByUser(DefaultUserID, tempStory.PostedBy, 0, DefaultPageSize, out total);

                Assert.IsTrue(stories.Length <= DefaultPageSize);

                for (int i = 0; i < stories.Length; i++)
                {
                    Assert.AreEqual(stories[i].PostedBy.Name, tempStory.User.UserName);
                }

                Assert.AreEqual(total, dataContext.Stories.Where(s => s.PostedBy == tempStory.PostedBy).Count());
            }
        }

        [Test]
        public void VerifySearchStories()
        {
            int total;
            const string query = "xbox";

            StoryListItem[] stories = dataContext.SearchStories(DefaultUserID, query, 0, DefaultPageSize, out total);

            if (stories.Length == 0)
            {
                Assert.AreEqual(total, 0);
            }
            else
            {
                Assert.IsTrue(stories.Length <= DefaultPageSize);

                for (int i = 0; i < stories.Length; i++)
                {
                    Assert.IsTrue(stories[i].Title.ToLowerInvariant().Contains(query) || stories[i].Description.ToLowerInvariant().Contains(query) || stories[i].Category.ToLowerInvariant().Contains(query) || (stories[i].Tags.Where(t => t.ToLowerInvariant().Contains(query)).Count() == 1));
                }

                Assert.AreEqual(total, dataContext.Stories.Where(s => (s.Title.Contains(query)) || (s.Description.Contains(query)) || (s.Category.Name.Contains(query)) || (s.StoryTags.Count(st => st.Tag.Name.Contains(query)) > 0)).Count());
            }
        }

        [Test]
        public void VerifySubmitStory()
        {
            const string url = "http://www.xxx.com";
            const string title = "Test Story";
            const string description = "Test Description";
            const string tags = "foo, bar";

            Story tempStory = dataContext.Stories.FirstOrDefault();

            if (tempStory != null)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    int id = dataContext.SubmitStory(url, title, 1, description, tags, tempStory.PostedBy);

                    Story story = dataContext.Stories.First(s => s.ID == id);

                    Assert.IsNotNull(story);
                    Assert.AreEqual(story.ID, id);
                    Assert.AreEqual(story.Url, url);
                    Assert.AreEqual(story.Title, title);
                    Assert.AreEqual(story.Description, description);
                    Assert.AreEqual(story.PostedBy, tempStory.PostedBy);
                }
            }
        }

        [Test]
        public void VerifyPostComment()
        {
            const string commentContent = "This is a Test Comment.";

            Story tempStory = dataContext.Stories.FirstOrDefault();

            if (tempStory != null)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    int id = dataContext.PostComment(tempStory.ID, tempStory.PostedBy, commentContent);

                    Comment comment = dataContext.Comments.Single(c => c.ID == id);

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