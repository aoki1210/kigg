namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    
    public interface IOrderedQuery<TEntity, TResult> : IQuery<TResult> 
        where TResult : class, IEnumerable<TEntity>
        where TEntity: class
    {
        IOrderedQuery<TEntity, TResult> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity, TResult> ThenBy<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity, TResult> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity, TResult> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity, TResult> Page(int pageIndex, int pageSize);
    }
}
