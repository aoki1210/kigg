namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IStoryRepository : IUniqueNameEntityRepository<IStory>
    {
        IStory FindByUrl(string url);

        PagedResult<IStory> FindPublished(int start, int max);

        PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max);

        PagedResult<IStory> FindUpcoming(int start, int max);

        PagedResult<IStory> FindByTag(Guid tagId, int start, int max);

        PagedResult<IStory> Search(string query, int start, int max);

        PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max);

        PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max);

        PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max);

        ICollection<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max);

        int CountByPublished();

        int CountByUpcoming();

        int CountByCategory(Guid categoryId);

        int CountByTag(Guid tagId);

        int CountByNew();

        int CountByPublishable(DateTime minimumDate, DateTime maximumDate);
    }
}