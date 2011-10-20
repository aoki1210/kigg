namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface ICommentRepository : IEntityRepository<Comment>, ICountByStoryRepository
    {
        PagedResult<Comment> FindAfter(long storyId, DateTime timestamp, int? start = null, int? max = null);
    }
}