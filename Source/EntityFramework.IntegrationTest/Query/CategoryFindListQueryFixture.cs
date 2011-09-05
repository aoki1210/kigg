namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using System.Linq;
    using System.Collections.Generic;
    
    using Xunit;
    using Xunit.Extensions;
    
    using DomainObjects;
    using EntityFramework.Query;

    public class CategoryFindListQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Excute_should_return_all_categories_when_no_filter_provided()
        {
            IList<Category> categories = NewCategoryList(true);

            var query = new CategoryFindListQuery(Context);

            var result = query.Execute();

            Assert.Equal(categories.Count, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Excute_should_return_correct_categories_list_when_filter_provided()
        {
            IList<Category> categories = NewCategoryList(true);
            
            var expectedCount = categories.Count(c => c.Id > 5);

            var query = new CategoryFindListQuery(Context, c => c.Id > 5);

            var result = query.Execute();

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Excute_should_return_correct_ordered_categories_list_when_orderd_decending_by_category_name()
        {
            NewCategoryList(true);

            var query = new CategoryFindListQuery(Context);

            var result = query.OrderByDescending(c => c.Name).Execute().ToList();

            long expectedHighestId = result.First().Id;
            long expectedLowestId = result.Last().Id;
            
            Assert.True(expectedHighestId > expectedLowestId);
        }
    }
}
