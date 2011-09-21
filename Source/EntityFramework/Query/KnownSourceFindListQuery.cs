namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class KnownSourceFindListQuery : OrderedQueryBase<KnownSource>
    {
        public KnownSourceFindListQuery(KiggDbContext context) : base(context)
        {
            Query = base.context.KnownSources;
        }

        public KnownSourceFindListQuery(KiggDbContext context, Expression<Func<KnownSource, bool>> predicate)
            : base(context)
        {
            Query = base.context.KnownSources.Where(predicate);
        }

        public override IEnumerable<KnownSource> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
