namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using System.Linq;
    using System.Collections.Generic;
    
    using Xunit;
    using Xunit.Extensions;
    
    using DomainObjects;
    using EntityFramework.Query;

    public class DomainObjectFindListQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Execute_should_return_all_users_when_no_filter_provided()
        {
            IList<User> users = NewUserList(true);

            var query = new DomainObjectFindListQuery<User>(Context);

            var result = query.Execute();

            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_users_list_when_filter_provided()
        {
            IList<User> users = NewUserList(true);
            
            var expectedCount = users.Count(c => c.Id > 5);

            var query = new DomainObjectFindListQuery<User>(Context, c => c.Id > 5);

            var result = query.Execute();

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_ordered_users_list_when_orderd_decending_by_username()
        {
            NewUserList(true);

            var query = new DomainObjectFindListQuery<User>(Context);

            var result = query.OrderByDescending(c => c.UserName).Execute().ToList();

            long expectedHighestId = result.First().Id;
            long expectedLowestId = result.Last().Id;

            Assert.True(expectedHighestId > expectedLowestId);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_all_categories_when_no_filter_provided()
        {
            IList<Category> categories = NewCategoryList(true);

            var query = new DomainObjectFindListQuery<Category>(Context);

            var result = query.Execute();

            Assert.Equal(categories.Count, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_categories_list_when_filter_provided()
        {
            IList<Category> categories = NewCategoryList(true);

            var expectedCount = categories.Count(c => c.Id > 5);

            var query = new DomainObjectFindListQuery<Category>(Context, c => c.Id > 5);

            var result = query.Execute();

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_ordered_categories_list_when_orderd_decending_by_category_name()
        {
            NewCategoryList(true);

            var query = new DomainObjectFindListQuery<Category>(Context);

            var result = query.OrderByDescending(c => c.Name).Execute().ToList();

            long expectedHighestId = result.First().Id;
            long expectedLowestId = result.Last().Id;

            Assert.True(expectedHighestId > expectedLowestId);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_all_tags_when_no_filter_provided()
        {
            IList<Tag> tags = NewTagList(true);

            var query = new DomainObjectFindListQuery<Tag>(Context);

            var result = query.Execute();

            Assert.Equal(tags.Count, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tags_list_when_filter_provided()
        {
            IList<Tag> tags = NewTagList(true);
            
            var expectedCount = tags.Count(t => t.Id > 5);

            var query = new DomainObjectFindListQuery<Tag>(Context, t => t.Id > 5);

            var result = query.Execute();

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_ordered_tags_list_when_orderd_decending_by_tag_name()
        {
            NewTagList(true);

            var query = new DomainObjectFindListQuery<Tag>(Context);

            var result = query.OrderByDescending(t => t.Name).Execute().ToList();

            long expectedHighestId = result.First().Id;
            long expectedLowestId = result.Last().Id;
            
            Assert.True(expectedHighestId > expectedLowestId);
        }
    }
}
