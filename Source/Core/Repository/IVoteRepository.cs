namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IVoteRepository : IRepository<Vote>, ICountByStoryRepository
    {
        Vote FindById(long storyId, long userId);

        IEnumerable<Vote> FindAfter(long storyId, DateTime timestamp);
    }
}