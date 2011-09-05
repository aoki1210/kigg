
namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    
    public interface IOrderedQuery<TEntity, TResult> : IQuery<TResult> 
        where TResult : class, IEnumerable<TEntity>
        where TEntity: class
    {
        IQuery<TResult> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        IQuery<TResult> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy);
    }
}
