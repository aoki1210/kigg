namespace Kigg.Repository.LinqToSql
{
    using System;
    using System.Linq;

    using DomainObjects;

    public class CommentSubscribtionRepository : BaseRepository<ICommentSubscribtion, CommentSubscribtion>, ICommentSubscribtionRepository
    {
        public CommentSubscribtionRepository(IDatabase database) : base(database)
        {
        }

        public CommentSubscribtionRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public int CountByStory(Guid storyId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");

            return Database.CommentSubscribtionDataSource.Count(cs => cs.StoryId == storyId);
        }

        public ICommentSubscribtion FindById(Guid storyId, Guid userId)
        {
            Check.Argument.IsNotEmpty(storyId, "storyId");
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.CommentSubscribtionDataSource.SingleOrDefault(cs => cs.StoryId == storyId && cs.UserId == userId);
        }
    }
}