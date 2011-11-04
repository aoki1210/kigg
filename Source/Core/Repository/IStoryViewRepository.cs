namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using Domain.Entities;

    public interface IStoryViewRepository : IRepository<StoryView>, ICountByStoryRepository
    {
        IEnumerable<StoryView> FindAfter(long storyId, DateTime timestamp);
    }
}