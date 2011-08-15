namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface ICommentRepository : IRepository<Comment>, ICountByStoryRepository
    {
        Comment FindById(long storyId, long commentId);

        ICollection<Comment> FindAfter(long storyId, DateTime timestamp);
    }
}