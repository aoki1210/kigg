namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IStoryViewRepository : IRepository<StoryView>, ICountByStoryRepository
    {
        IEnumerable<StoryView> FindAfter(long storyId, DateTime timestamp);
    }
}