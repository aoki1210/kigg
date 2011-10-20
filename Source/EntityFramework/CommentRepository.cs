namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using Query;
    using Repository;
    using DomainObjects;

    public class CommentRepository : EntityRepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public int CountByStory(long storyId)
        {
            Check.Argument.IsNotNegativeOrZero(storyId, "storyId");

            var query = QueryFactory.CreateCountCommentsByStory(storyId);

            return query.Execute();
        }

        public PagedResult<Comment> FindAfter(long storyId, DateTime timestamp, int? start, int? max)
        {
            Check.Argument.IsNotNegativeOrZero(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(timestamp, "timestamp");

            var query = QueryFactory.CreateFindCommentsForStoryAfterDate(storyId, timestamp, start, max);

            return CreatePagedResult(query);
        }
    }
}