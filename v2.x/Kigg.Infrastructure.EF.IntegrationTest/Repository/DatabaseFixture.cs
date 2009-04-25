using System;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class DatabaseFixture : BaseIntegrationFixture, IDisposable
    {
        private readonly Database _database;

        public DatabaseFixture()
        {
            _database = new Database(ConnectionString);
        }

        [Fact]
        public void ObjectContext_With_DataLoadOptions_Should_Return_Correct_Object_Graph()
        {
            var options = new DataLoadOptions();
            _database.LoadOptions = options;

            options.LoadWith<Story>(s => s.User);
            options.LoadWith<Story>(s => s.Category);
            options.LoadWith<Story>(s => s.StoryComments);
            options.LoadWith<Story>(s => s.StoryVotes);
            options.LoadWith<Story>(s => s.StoryViews);
            options.LoadWith<Story>(s => s.StoryMarkAsSpams);
            options.LoadWith<Story>(s => s.CommentSubscribers);
            var stories = _database.StoryDataSource;
            foreach(var story in stories)
            {
                Assert.True(story.UserReference.IsLoaded);
                Assert.True(story.CategoryReference.IsLoaded);
                Assert.True(story.StoryComments.IsLoaded);
                Assert.True(story.StoryVotes.IsLoaded);
                Assert.True(story.StoryViews.IsLoaded);
                Assert.True(story.StoryMarkAsSpams.IsLoaded);
                Assert.True(story.CommentSubscribers.IsLoaded);
            }

            options.LoadWith<User>(s => s.Stories);
            options.LoadWith<User>(s => s.Spams);
            options.LoadWith<User>(s => s.StoryVotes);
            options.LoadWith<User>(s => s.SubmittedComments);
            var users = _database.UserDataSource;
            foreach(var user in users)
            {
                Assert.True(user.SubmittedComments.IsLoaded);
                Assert.True(user.StoryVotes.IsLoaded);
                Assert.True(user.Stories.IsLoaded);
                Assert.True(user.Spams.IsLoaded);
                Assert.False(user.UserTags.IsLoaded);
            }

            options.LoadWith<Category>(c => c.Stories);
            var categories = _database.CategoryDataSource;
            foreach (var category in categories)
            {
                Assert.True(category.Stories.IsLoaded);
            }
        }
        
        public void Dispose()
        {
            _database.Dispose();
        }
    }
}