using System;
using System.Linq;
using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;
    using System.Transactions;

    public class DatabaseFixture : BaseIntegrationFixture, IDisposable
    {
        [Fact]
        public void ObjectContext_With_LoadOptions_Should_Preload_UserTags_When_User_Is_Loaded()
        {
            var options = new DataLoadOptions();
            _database.LoadOptions = options;

            options.LoadWith<User>(u => u.UserTagsInternal);
            var users = _database.UserDataSource;
            foreach(var user in users)
            {
                Assert.True(user.UserTags.IsLoaded);
                Assert.Equal(user.Tags.Count, user.TagCount);
            }
        }
        
        [Fact]
        public void DeleteAllOnSubmit_And_Presist_Changes_Should_Correctly_Update_Database()
        {
            var votes = _database.VoteDataSource;
            using(new TransactionScope())
            {
                Assert.True(votes.Count() > 0);

                _database.DeleteAllOnSubmit(votes);
                _database.SubmitChanges();

                Assert.True(votes.Count() == 0);
            }
        }
        [Fact]
        public void StorySearchResult_Should_Return_Correct_Search_Results()
        {
            _database.SetSearchQuery("mvc");
            var results = _database.StorySearchResult;
            Assert.True(results.Count() >= 0);
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}