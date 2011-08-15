namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IMarkAsSpamRepository : IRepository<MarkAsSpam>, ICountByStoryRepository
    {
        MarkAsSpam FindById(long storyId, long userId);

        ICollection<MarkAsSpam> FindAfter(long storyId, DateTime timestamp);
    }
}