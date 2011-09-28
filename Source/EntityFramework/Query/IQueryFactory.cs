namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq.Expressions;

    using DomainObjects;

    public interface IQueryFactory
    {
        bool UseCompiled { get; }

        IQuery<decimal> CreateCalculateUserScoreById(long id, DateTime startDate, DateTime endDate);
        IQuery<int> CreateCountVotesByStoryId(long id);
        IQuery<int> CreateCountStoryViewsByStoryId(long id);
        IQuery<Category> CreateFindUniqueCategoryByName(string name);
        IQuery<Category> CreateFindUniqueCategoryByUniqueName(string uniqueName);
        IOrderedQuery<Category> CreateFindAllCategories<TKey>(Expression<Func<Category, TKey>> orderBy);
        IQuery<User> CreateFindUserById(long id);
        IQuery<User> CreateFindUserByEmail(string email);
        IQuery<User> CreateFindUserByUserName(string userName);
        IQuery<Story> CreateFindStoryById(long id);
        IQuery<Story> CreateFindStoryByUniqueName(string uniqueName);
        IQuery<Story> CreateFindStoryByUrl(string url);
        IOrderedQuery<User> CreateFindTopScoredUsers(DateTime startDate, DateTime endDate, int start, int max);
        IOrderedQuery<User> CreateFindAllUsers<TKey>(int start, int max, Expression<Func<User, TKey>> orderBy);
        IQuery<Tag> CreateFindTagByUniqueName(string uniqueName);
        IQuery<Tag> CreateFindTagByName(string name);
        IOrderedQuery<Tag> CreateFindTagsByMatchingName(string name, int max);
        IOrderedQuery<Tag> CreateFindTagsByUsage(int max);
        IOrderedQuery<Tag> CreateFindAllTags<TKey>(Expression<Func<Tag, TKey>> orderBy);
        IQuery<KnownSource> CreateFindKnownSourceByUrl(string url);
        IOrderedQuery<KnownSource> CreateFindAllKnownSources<TKey>(Expression<Func<KnownSource, TKey>> orderBy);
        IQuery<Vote> CreateFindVoteById(long userId, long storyId);        
        IOrderedQuery<Vote> CreateFindStoryVotesAfterDate(long storyId, DateTime date);
        IOrderedQuery<StoryView> CreateFindStoryViewsAfterDate(long storyId, DateTime date);
        IQuery<Comment> CreateFindCommentById(long id);
        IOrderedQuery<Comment> CreateFindCommentsForStoryAfterDate(long storyId, DateTime date, int start, int max);
    }
}