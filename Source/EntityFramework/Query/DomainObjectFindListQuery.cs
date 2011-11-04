namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using Domain.Entities;

    public class DomainObjectFindListQuery<TResult> : OrderedQueryBase<TResult> where TResult:class, IDomainObject
    {
        public DomainObjectFindListQuery(KiggDbContext context, bool useCompiled) : base(context, useCompiled)
        {
        }

        public DomainObjectFindListQuery(KiggDbContext context) : base(context)
        {
        }

        public DomainObjectFindListQuery(KiggDbContext context, Expression<Func<TResult, bool>> predicate) : base(context, predicate)
        {
        }

        public override IEnumerable<TResult> Execute()
        {
            return Query.ToList();
        }
    }
}
