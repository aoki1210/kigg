namespace Kigg.Repository
{
    using System;

    using Domain.Entities;

    public interface IStoryRepository : IUniqueNameEntityRepository<Story>
    {
        Story FindByUrl(string url);

        PagedResult<Story> FindPublished(int start, int max);

        PagedResult<Story> FindPublishedByCategory(long categoryId, int start, int max);

        PagedResult<Story> FindPublishedByCategory(string category, int start, int max);

        PagedResult<Story> FindUpcoming(int start, int max);

        PagedResult<Story> FindNew(int start, int max);

        PagedResult<Story> FindUnapproved(int start, int max);

        PagedResult<Story> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max);

        PagedResult<Story> FindByTag(long tagId, int start, int max);

        PagedResult<Story> FindByTag(string tag, int start, int max);

        PagedResult<Story> FindPostedByUser(long userId, int start, int max);

        PagedResult<Story> FindPostedByUser(string userName, int start, int max);

        PagedResult<Story> FindPromotedByUser(long userId, int start, int max);

        PagedResult<Story> FindPromotedByUser(string userName, int start, int max);

        PagedResult<Story> FindCommentedByUser(long userId, int start, int max);

        int CountPublished();

        int CountUpcoming();

        int CountByCategory(long categoryId);

        int CountByTag(long tagId);

        int CountNew();

        int CountUnapproved();

        int CountPublishable(DateTime minimumDate, DateTime maximumDate);

        int CountPostedByUser(long userId);
    }
}