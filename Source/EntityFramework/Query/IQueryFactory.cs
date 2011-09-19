namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq.Expressions;

    using DomainObjects;

    public interface IQueryFactory
    {
        bool UseCompiled { get; }

        IQuery<User> CreateFindUserByEmail(string email);
        IQuery<User> CreateFindUserByUserName(string userName);
        IOrderedQuery<User> CreateFindTopScoredUsers(DateTime startDate, DateTime endDate, int start, int max);
        IOrderedQuery<User> CreateFindAllUsers<TKey>(int start, int max, Expression<Func<User, TKey>> orderBy);
        IQuery<decimal> CreateCalculateUserScoreById(long id, DateTime startDate, DateTime endDate);
        IQuery<Tag> CreateFindTagByUniqueName(string uniqueName);
        IQuery<Tag> CreateFindTagByName(string name);
        IOrderedQuery<Tag> CreateFindTagsByMatchingName(string name, int max);
        IOrderedQuery<Tag> CreateFindTagsByUsage(int max);
        IOrderedQuery<Tag> CreateFindAllTags<TKey>(Expression<Func<Tag, TKey>> orderBy);
    }
}