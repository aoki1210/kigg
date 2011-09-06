namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using Xunit;
    using Xunit.Extensions;

    using EntityFramework.Query;

    public class UserFindByUniqueKeyQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_user_when_fetching_by_id()
        {
            var user = NewUser(true);

            var query = new UserFindByUniqueKeyQuery(Context, c => c.Id == user.Id);

            var result = query.Execute();

            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_user_when_fetching_by_username()
        {
            var user = NewUser(true);

            var query = new UserFindByUniqueKeyQuery(Context, c => c.UserName == user.UserName);

            var result = query.Execute();

            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_user_when_fetching_by_email()
        {
            var user = NewUser(true);

            var query = new UserFindByUniqueKeyQuery(Context, c => c.Email == user.Email);

            var result = query.Execute();

            Assert.Equal(user.Id, result.Id);
        }
    }
}
