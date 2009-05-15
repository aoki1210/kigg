using System;
using System.Linq;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    
    public class DatabaseFactoryFixture : IDisposable
    {
        private readonly DatabaseFactory _factory;
     
        public DatabaseFactoryFixture()
        {
            var connectionString = new ConnectionString(new ConfigurationManagerWrapper(),"KiGG");
            _factory = new DatabaseFactory(connectionString);
        }
        [Fact]
        public void Get_Should_Return_Correctly_Initialized_Database()
        {
            using(var db = _factory.Get())
            {
                var story = db.StoryDataSource.First();
                var vote = db.VoteDataSource.First();
                var spamMark = db.MarkAsSpamDataSource.FirstOrDefault();
                var comment = db.CommentDataSource.First();

                Assert.True(story.UserReference.IsLoaded);
                Assert.True(story.CategoryReference.IsLoaded);
                Assert.True(story.StoryTagsInternal.IsLoaded);

                Assert.True(vote.UserReference.IsLoaded);
                
                if(spamMark!=null)
                    Assert.True(spamMark.UserReference.IsLoaded);

                Assert.True(comment.UserReference.IsLoaded);
            }
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
