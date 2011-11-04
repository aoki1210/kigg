﻿
namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    
    using Domain.Entities;

    public abstract class OrderedQueryBase<TResult> : QueryBase<IEnumerable<TResult>>, IOrderedQuery<TResult>
        where TResult : class, IDomainObject
    {
        protected IQueryable<TResult> OriginalQuery { get; set; }

        protected IQueryable<TResult> Query { get; set; }

        protected OrderedQueryBase(KiggDbContext context, bool useCompiled)
            : base(context, useCompiled)
        {
            OriginalQuery = context.Set<TResult>();
            Query = OriginalQuery;
        }

        protected OrderedQueryBase(KiggDbContext context)
            : this(context, true)
        {
        }

        protected OrderedQueryBase(KiggDbContext context, Expression<Func<TResult, bool>> predicate)
            : this(context, true)
        {
            OriginalQuery = context.Set<TResult>().Where(predicate);
            Query = OriginalQuery;
        }

        public virtual int Count()
        {
            return OriginalQuery.Count();
        }
        public virtual IOrderedQuery<TResult> OrderBy<TKey>(Expression<Func<TResult, TKey>> orderBy)
        {
            Query = Query.OrderBy(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TResult> ThenBy<TKey>(Expression<Func<TResult, TKey>> orderBy)
        {
            Query = ((IOrderedQueryable<TResult>)Query).ThenBy(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TResult> OrderByDescending<TKey>(Expression<Func<TResult, TKey>> orderBy)
        {
            Query = Query.OrderByDescending(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TResult> ThenByDescending<TKey>(Expression<Func<TResult, TKey>> orderBy)
        {
            Query = ((IOrderedQueryable<TResult>)Query).ThenByDescending(orderBy);
            return this;
        }
        public virtual IOrderedQuery<TResult> Page(int pageIndex, int pageSize)
        {
            Check.Argument.IsNotNegative(pageIndex, "pageIndex");
            Check.Argument.IsNotZeroOrNegative(pageSize, "pageSize");

            Query = Query.Skip(pageIndex).Take(pageSize);
            return this;
        }
        public virtual IOrderedQuery<TResult> Limit(int max)
        {
            Check.Argument.IsNotZeroOrNegative(max, "max");

            Query = Query.Take(max);
            return this;
        }
    }
}
