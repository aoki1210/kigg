namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using Xunit;
    using Xunit.Extensions;

    using EntityFramework.Query;

    public class CategoryByIdQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Excute_should_return_correct_category()
        {
            var category = NewCategory(true);

            var query = new CategoryByIdQuery(category.Id);

            var result = query.Execute(Context);

            Assert.Equal(category.Id, result.Id);
        }
    }
}
