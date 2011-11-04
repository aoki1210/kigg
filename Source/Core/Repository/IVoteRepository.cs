namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using Domain.Entities;

    public interface IVoteRepository : IRepository<Vote>, ICountByStoryRepository
    {
        Vote FindById(long storyId, long userId);

        IEnumerable<Vote> FindAfter(long storyId, DateTime timestamp);
    }
}