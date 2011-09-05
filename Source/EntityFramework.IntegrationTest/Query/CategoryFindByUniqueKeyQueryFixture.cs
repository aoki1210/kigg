﻿namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using Xunit;
    using Xunit.Extensions;

    using EntityFramework.Query;

    public class CategoryFindByUniqueKeyQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Excute_should_return_correct_category_when_fetching_by_id()
        {
            var category = NewCategory(true);

            var query = new CategoryFindByUniqueKeyQuery(Context, c => c.Id == category.Id);

            var result = query.Execute();

            Assert.Equal(category.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Excute_should_return_correct_category_when_fetching_by_unique_name()
        {
            var category = NewCategory(true);

            var query = new CategoryFindByUniqueKeyQuery(Context, c => c.UniqueName == category.UniqueName);

            var result = query.Execute();

            Assert.Equal(category.Id, result.Id);
        }
    }
}
