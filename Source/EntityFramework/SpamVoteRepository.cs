namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using Query;
    using Repository;
    using Domain.Entities;

    public class SpamVoteRepository : DomainObjectRepositoryBase<SpamVote>, ISpamVoteRepository
    {
        public SpamVoteRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public int CountByStory(long storyId)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");

            var query = QueryFactory.CreateCountSpamVotesByStory(storyId);

            return query.Execute();
        }


        public SpamVote FindById(long storyId, long userId)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotZeroOrNegative(userId, "userId");

            var query = QueryFactory.CreateFindSpamVoteById(storyId, userId);

            return query.Execute();
        }

        public IEnumerable<SpamVote> FindAfter(long storyId, DateTime timestamp)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            var query = QueryFactory.CreateFindSpamVotesAfterDate(storyId, timestamp);

            return query.Execute();
        }
    }
}