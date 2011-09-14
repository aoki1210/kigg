namespace Kigg.Infrastructure.EntityFramework.IntegrationTest.Query
{
    using System.Linq;
    using System.Collections.Generic;
    
    using Xunit;
    using Xunit.Extensions;
    
    using DomainObjects;
    using EntityFramework.Query;

    public class TagFindListQueryFixture : IntegrationFixtureBase
    {
        [Fact]
        [AutoRollback]
        public void Execute_should_return_all_tags_when_no_filter_provided()
        {
            IList<Tag> tags = NewTagList(true);

            var query = new TagFindListQuery(Context);

            var result = query.Execute();

            Assert.Equal(tags.Count, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_tags_list_when_filter_provided()
        {
            IList<Tag> tags = NewTagList(true);
            
            var expectedCount = tags.Count(t => t.Id > 5);

            var query = new TagFindListQuery(Context, t => t.Id > 5);

            var result = query.Execute();

            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        [AutoRollback]
        public void Execute_should_return_correct_ordered_tags_list_when_orderd_decending_by_tag_name()
        {
            NewTagList(true);

            var query = new TagFindListQuery(Context);

            var result = query.OrderByDescending(t => t.Name).Execute().ToList();

            long expectedHighestId = result.First().Id;
            long expectedLowestId = result.Last().Id;
            
            Assert.True(expectedHighestId > expectedLowestId);
        }
    }
}
