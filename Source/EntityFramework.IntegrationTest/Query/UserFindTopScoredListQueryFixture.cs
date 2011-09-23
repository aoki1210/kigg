namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using System.Linq;

    using Xunit;
    using Xunit.Extensions;

    using EntityFramework.Query;

    public class UserFindTopScoredListQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_list_of_top_scored_users_in_the_last_10_days()
        {
            var users = NewUserList(true);
            var user1 = users.First();
            var user2 = users.Last();
            //Generates 5 score entries each equal to 10pts for the last 5 days (including today)
            GenerateScoreForUser(user1, 30, 5, true);
            //Generates 10 score entries each equal to 10pts for the last 10 days (including today)
            GenerateScoreForUser(user2, 10, 10, true);

            var startDate = SystemTime.Now().AddDays(-10);
            var endDate = SystemTime.Now();
            var query = new UserFindTopScoredListQuery(Context, us =>
                                                        us.CreatedAt >= startDate && us.CreatedAt <= endDate);
            var result = query.Execute().ToList();
            var topUser = result.First();
            Assert.Equal(user1.Id, topUser.Id);
        }
    }
}
