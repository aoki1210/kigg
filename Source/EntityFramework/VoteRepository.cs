namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using Query;    
    using Repository;
    using Domain.Entities;

    public class VoteRepository : DomainObjectRepositoryBase<Vote>, IVoteRepository
    {
        public VoteRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public int CountByStory(long storyId)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            
            var query = QueryFactory.CreateCountVotesByStory(storyId);

            return query.Execute();
        }

        public Vote FindById(long storyId, long userId)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotZeroOrNegative(userId, "userId");

            var query = QueryFactory.CreateFindVoteById(storyId, userId);

            return query.Execute();
        }

        public IEnumerable<Vote> FindAfter(long storyId, DateTime timestamp)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            var query = QueryFactory.CreateFindVotesAfterDate(storyId, timestamp);

            return query.Execute();
        }
    }
}