namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using Xunit;
    using Xunit.Extensions;

    using EntityFramework.Query;

    public class TagFindUniqueQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tag_when_fetching_by_id()
        {
            var tag = NewTag(true);

            var query = new TagFindUniqueQuery(Context, t => t.Id == tag.Id);

            var result = query.Execute();

            Assert.Equal(tag.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tag_when_fetching_by_unique_name()
        {
            var tag = NewTag(true);

            var query = new TagFindUniqueQuery(Context, t => t.UniqueName == tag.UniqueName);

            var result = query.Execute();

            Assert.Equal(tag.Id, result.Id);
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tag_when_fetching_by_name()
        {
            var tag = NewTag(true);

            var query = new TagFindUniqueQuery(Context, t => t.Name == tag.Name);

            var result = query.Execute();

            Assert.Equal(tag.Id, result.Id);
        }
    }
}
