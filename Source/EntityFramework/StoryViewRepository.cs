namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    using Query;
    using Repository;
    using DomainObjects;

    public class StoryViewRepository : DomainObjectRepositoryBase<StoryView>, IStoryViewRepository
    {
        public StoryViewRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public int CountByStory(long storyId)
        {
            Check.Argument.IsNotNegativeOrZero(storyId, "storyId");
            
            var query = QueryFactory.CreateCountViewsByStory(storyId);

            return query.Execute();
        }

        public IEnumerable<StoryView> FindAfter(long storyId, DateTime timestamp)

        {
            Check.Argument.IsNotNegativeOrZero(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            var query = QueryFactory.CreateFindStoryViewsAfterDate(storyId, timestamp);

            return query.Execute();
        }
    }
}