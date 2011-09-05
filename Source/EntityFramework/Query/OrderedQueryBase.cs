namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    
    public abstract class OrderedQueryBase<TEntity, TResult> : QueryBase<TResult>, IOrderedQuery<TEntity, TResult>
        where TResult : class, IEnumerable<TEntity>
        where TEntity : class
    {
        protected OrderedQueryBase(KiggDbContext context, bool useCompiled) : base(context, useCompiled)
        {
        }

        protected OrderedQueryBase(KiggDbContext context) : base(context)
        {
        }

        public abstract IQuery<TResult> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderBy);
        public abstract IQuery<TResult> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy);   
    }
}
