namespace Kigg.Repository
{
    using System;

    using DomainObjects;

    public interface ICommentSubscribtionRepository : IRepository<CommentSubscribtion>, ICountByStoryRepository
    {
        CommentSubscribtion FindById(long storyId, long userId);
    }
}