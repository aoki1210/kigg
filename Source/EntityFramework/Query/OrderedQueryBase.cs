using System.Linq;

namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public abstract class OrderedQueryBase<TEntity, TResult> : QueryBase<TResult>, IOrderedQuery<TEntity, TResult>
        where TResult : class, IEnumerable<TEntity>
        where TEntity : class
    {
        protected IQueryable<TEntity> Query { get; set; }

        protected OrderedQueryBase(KiggDbContext context, bool useCompiled)
            : base(context, useCompiled)
        {
        }

        protected OrderedQueryBase(KiggDbContext context)
            : base(context)
        {
        }

        public virtual IOrderedQuery<TEntity, TResult> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = Query.OrderBy(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity, TResult> ThenBy<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = ((IOrderedQueryable<TEntity>)Query).ThenBy(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity, TResult> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = Query.OrderByDescending(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity, TResult> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = ((IOrderedQueryable<TEntity>)Query).ThenByDescending(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity, TResult> Page(int pageIndex, int pageSize)
        {
            Check.Argument.IsNotNegative(pageIndex, "pageIndex");
            Check.Argument.IsNotNegative(pageSize, "pageSize");

            Query = Query.Skip(pageIndex).Take(pageSize);
            return this;
        }
    }
}
