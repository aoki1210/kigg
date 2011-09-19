using System.Linq;

namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public abstract class OrderedQueryBase<TEntity> : QueryBase<IEnumerable<TEntity>>, IOrderedQuery<TEntity>
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

        public virtual long Count()
        {
            return Query.Count();
        }
        public virtual IOrderedQuery<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = Query.OrderBy(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity> ThenBy<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = ((IOrderedQueryable<TEntity>)Query).ThenBy(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = Query.OrderByDescending(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity> ThenByDescending<TKey>(Expression<Func<TEntity, TKey>> orderBy)
        {
            Query = ((IOrderedQueryable<TEntity>)Query).ThenByDescending(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TEntity> Page(int pageIndex, int pageSize)
        {
            Check.Argument.IsNotNegative(pageIndex, "pageIndex");
            Check.Argument.IsNotNegativeOrZero(pageSize, "pageSize");

            Query = Query.Skip(pageIndex).Take(pageSize);
            return this;
        }
        public virtual IOrderedQuery<TEntity> Limit(int max)
        {
            Check.Argument.IsNotNegativeOrZero(max, "max");
            
            Query = Query.Take(max);
            return this;
        }
    }
}
