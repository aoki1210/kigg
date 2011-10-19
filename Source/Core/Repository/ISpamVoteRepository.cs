namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface ISpamVoteRepository : IRepository<SpamVote>, ICountByStoryRepository
    {
        SpamVote FindById(long storyId, long userId);

        IEnumerable<SpamVote> FindAfter(long storyId, DateTime timestamp);
    }
}