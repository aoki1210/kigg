namespace Kigg.Repository
{
    using System;

    using Domain.Entities;

    public interface ICommentSubscribtionRepository : IRepository<CommentSubscribtion>, ICountByStoryRepository
    {
        CommentSubscribtion FindById(long storyId, long userId);
    }
}