namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq.Expressions;

    using Domain.Entities;

    public interface IQueryFactory
    {
        bool UseCompiled { get; }

        IQuery<decimal> CreateCalculateUserScoreById(long id, DateTime startDate, DateTime endDate);
        IQuery<int> CreateCountCommentsByStory(long id);
        IQuery<int> CreateCountVotesByStory(long id);
        IQuery<int> CreateCountSpamVotesByStory(long id);
        IQuery<int> CreateCountViewsByStory(long id);
        IQuery<int> CreateCountNewStories();
        IQuery<int> CreateCountUpcomingStories();
        IQuery<int> CreateCountUnapprovedStories();
        IQuery<int> CreateCountPublishableStories(DateTime minimumDate, DateTime maximumDate);
        IQuery<int> CreateCountPublishedStories();
        IQuery<int> CreateCountPublishedStoriesByCategory(long categoryId);
        IQuery<int> CreateCountPublishedStoriesByCategory(string category);
        IQuery<int> CreateCountStoriesByTag(long tagId);
        IQuery<int> CreateCountStoriesByTag(string tag);
        IQuery<int> CreateCountPostedStoriesByUser(long userId);
        IQuery<int> CreateCountPostedStoriesByUser(string userName);
        IQuery<Category> CreateFindUniqueCategoryByName(string name);
        IQuery<Category> CreateFindUniqueCategoryByUniqueName(string uniqueName);
        IOrderedQuery<Category> CreateFindAllCategories<TKey>(Expression<Func<Category, TKey>> orderBy);
        IQuery<User> CreateFindUserById(long id);
        IQuery<User> CreateFindUserByEmail(string email);
        IQuery<User> CreateFindUserByUserName(string userName);
        IQuery<Story> CreateFindStoryById(long id);
        IQuery<Story> CreateFindStoryByUniqueName(string uniqueName);
        IQuery<Story> CreateFindStoryByUrl(string urlHash);
        IOrderedQuery<Story> CreateFindPublishedStories(int start, int max);
        IOrderedQuery<Story> CreateFindPublishedStoriesByCategory(long categoryId, int start, int max);
        IOrderedQuery<Story> CreateFindPublishedStoriesByCategory(string category, int start, int max);
        IOrderedQuery<Story> CreateFindUpcomingStories(int start, int max);
        IOrderedQuery<Story> CreateFindNewStories(int start, int max);
        IOrderedQuery<Story> CreateFindUnapprovedStories(int start, int max);
        IOrderedQuery<Story> CreateFindPublishableStories(DateTime minimumDate, DateTime maximumDate, int start, int max);
        IOrderedQuery<Story> CreateFindStoriesByTag(long tagId, int start, int max);
        IOrderedQuery<Story> CreateFindStoriesByTag(string tag, int start, int max);
        IOrderedQuery<Story> CreateFindPostedStoriesByUser(long userId, int start, int max);
        IOrderedQuery<Story> CreateFindPostedStoriesByUser(string userName, int start, int max);
        IOrderedQuery<Story> CreateFindPromotedStoriesByUser(long userId, int start, int max);
        IOrderedQuery<Story> CreateFindPromotedStoriesByUser(string userName, int start, int max);
        IOrderedQuery<Story> CreateFindCommentedStoriesByUser(long userId, int start, int max);
        IOrderedQuery<User> CreateFindTopScoredUsers(DateTime startDate, DateTime endDate, int start, int max);
        IOrderedQuery<User> CreateFindAllUsers<TKey>(int start, int max, Expression<Func<User, TKey>> orderBy);
        IQuery<Tag> CreateFindUniqueTagByUniqueName(string uniqueName);
        IQuery<Tag> CreateFindUniqueTagByName(string name);
        IOrderedQuery<Tag> CreateFindTagsByMatchingName(string name, int max);
        IOrderedQuery<Tag> CreateFindTagsByUsage(int max);
        IOrderedQuery<Tag> CreateFindAllTags<TKey>(Expression<Func<Tag, TKey>> orderBy);
        IQuery<KnownSource> CreateFindKnownSourceByUrl(string url);
        IOrderedQuery<KnownSource> CreateFindAllKnownSources<TKey>(Expression<Func<KnownSource, TKey>> orderBy);
        IQuery<Vote> CreateFindVoteById(long userId, long storyId);        
        IOrderedQuery<Vote> CreateFindVotesAfterDate(long storyId, DateTime date);
        IQuery<SpamVote> CreateFindSpamVoteById(long userId, long storyId);
        IOrderedQuery<SpamVote> CreateFindSpamVotesAfterDate(long storyId, DateTime date);
        IOrderedQuery<StoryView> CreateFindStoryViewsAfterDate(long storyId, DateTime date);
        IQuery<Comment> CreateFindCommentById(long id);
        IOrderedQuery<Comment> CreateFindCommentsForStoryAfterDate(long storyId, DateTime date, int? start = null, int? max = null);
    }
}