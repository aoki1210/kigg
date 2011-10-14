namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using Query;    
    using Repository;
    using DomainObjects;

    public class VoteRepository : DomainObjectRepositoryBase<Vote>, IVoteRepository
    {
        public VoteRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public int CountByStory(long storyId)
        {
            Check.Argument.IsNotNegativeOrZero(storyId, "storyId");
            
            var query = QueryFactory.CreateCountVotesByStory(storyId);

            return query.Execute();
        }

        public Vote FindById(long storyId, long userId)
        {
            Check.Argument.IsNotNegativeOrZero(storyId, "storyId");
            Check.Argument.IsNotNegativeOrZero(userId, "userId");

            var query = QueryFactory.CreateFindVoteById(storyId, userId);

            return query.Execute();
        }

        public IEnumerable<Vote> FindAfter(long storyId, DateTime timestamp)
        {
            Check.Argument.IsNotNegativeOrZero(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            var query = QueryFactory.CreateFindVotesAfterDate(storyId, timestamp);

            return query.Execute();
        }
    }
}