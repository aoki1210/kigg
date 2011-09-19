namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    
    public interface IOrderedQuery<TEntity> : IQuery<IEnumerable<TEntity>> 
        where TEntity: class
    {
        long Count();
        IOrderedQuery<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IOrderedQuery<TEntity> Page(int pageIndex, int pageSize);
        IOrderedQuery<TEntity> Limit(int max);
    }
}
