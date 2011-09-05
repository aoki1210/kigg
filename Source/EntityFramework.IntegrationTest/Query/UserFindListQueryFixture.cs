namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using System.Linq;
    using System.Collections.Generic;
    
    using Xunit;
    using Xunit.Extensions;
    
    using DomainObjects;
    using EntityFramework.Query;

    public class UserFindListQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Excute_should_return_all_users_when_no_filter_provided()
        {
            IList<User> users = NewUserList(true);

            var query = new UserFindListQuery(Context);

            var result = query.Execute();

            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Excute_should_return_correct_users_list_when_filter_provided()
        {
            IList<User> users = NewUserList(true);
            
            var expectedCount = users.Count(c => c.Id > 5);

            var query = new UserFindListQuery(Context, c => c.Id > 5);

            var result = query.Execute();

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Excute_should_return_correct_ordered_users_list_when_orderd_decending_by_username()
        {
            NewUserList(true);

            var query = new UserFindListQuery(Context);

            var result = query.OrderByDescending(c => c.UserName).Execute().ToList();

            long expectedHighestId = result.First().Id;
            long expectedLowestId = result.Last().Id;

            Assert.True(expectedHighestId > expectedLowestId);
        }
    }
}
