namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using System;
    using System.Linq;

    using FizzWare.NBuilder.Generators;

    using Xunit;
    using Xunit.Extensions;

    using Domain.Entities;

    using EntityFramework.Query;

    public class UserFindTopScoredListQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_list_of_top_scored_users_in_the_last_10_days()
        {
            var users = NewDomainObjectList<User>();
            var user1 = users.First();
            var user2 = users.Last();
            DateTime now = SystemTime.Now;

            //Generates 5 score entries each equal to 10pts for the last 5 days
            NewDomainObjectList<UserScore>(5, c =>
                                                  {
                                                      c.ScoredBy = user1;
                                                      c.Action = UserAction.StorySubmitted;
                                                      c.Score = 5;
                                                      c.CreatedAt =
                                                          GetRandom.DateTime(now.AddDays(-5), now);
                                                  });

            //Generates 10 score entries each equal to 10pts for the last 10 days
            NewDomainObjectList<UserScore>(10, c =>
                                                   {
                                                       c.ScoredBy = user2;
                                                       c.Action = UserAction.StorySubmitted;
                                                       c.Score = 10;
                                                       c.CreatedAt =
                                                           GetRandom.DateTime(now.AddDays(-10), now);
                                                   });

            var startDate = now.AddDays(-10);
            var query = new UserFindTopScoredListQuery(Context, us =>
                                                        us.CreatedAt >= startDate && us.CreatedAt <= now);
            var result = query.Execute().ToList();
            var topUser = result.First();
            Assert.Equal(user2.Id, topUser.Id);
        }
    }
}
