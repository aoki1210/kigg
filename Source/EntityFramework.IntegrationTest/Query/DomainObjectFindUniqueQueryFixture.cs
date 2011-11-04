namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using Xunit;
    using Xunit.Extensions;

    using Domain.Entities;
    using EntityFramework.Query;

    public class DomainObjectFindUniqueQuery : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_category_when_fetching_by_id()
        {
            var category = NewCategory(true);

            var query = new DomainObjectFindUniqueQuery<Category>(Context, c => c.Id == category.Id);

            var result = query.Execute();

            Assert.Equal(category.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_category_when_fetching_by_unique_name()
        {
            var category = NewCategory(true);

            var query = new DomainObjectFindUniqueQuery<Category>(Context, c => c.UniqueName == category.UniqueName);

            var result = query.Execute();

            Assert.Equal(category.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tag_when_fetching_by_id()
        {
            var tag = NewTag(true);

            var query = new DomainObjectFindUniqueQuery<Tag>(Context, t => t.Id == tag.Id);

            var result = query.Execute();

            Assert.Equal(tag.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tag_when_fetching_by_unique_name()
        {
            var tag = NewTag(true);

            var query = new DomainObjectFindUniqueQuery<Tag>(Context, t => t.UniqueName == tag.UniqueName);

            var result = query.Execute();

            Assert.Equal(tag.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tag_when_fetching_by_name()
        {
            var tag = NewTag(true);

            var query = new DomainObjectFindUniqueQuery<Tag>(Context, t => t.Name == tag.Name);

            var result = query.Execute();

            Assert.Equal(tag.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_user_when_fetching_by_id()
        {
            var user = NewUser(true);

            var query = new DomainObjectFindUniqueQuery<User>(Context, c => c.Id == user.Id);

            var result = query.Execute();

            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_user_when_fetching_by_username()
        {
            var user = NewUser(true);

            var query = new DomainObjectFindUniqueQuery<User>(Context, c => c.UserName == user.UserName);

            var result = query.Execute();

            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_user_when_fetching_by_email()
        {
            var user = NewUser(true);

            var query = new DomainObjectFindUniqueQuery<User>(Context, c => c.Email == user.Email);

            var result = query.Execute();

            Assert.Equal(user.Id, result.Id);
        }
    }
}
