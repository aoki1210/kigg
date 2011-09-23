namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;
    public class VoteFindListQuery : OrderedQueryBase<Vote>
    {
        public VoteFindListQuery(KiggDbContext context) : base(context)
        {
            Query = base.context.Votes;
        }

        public VoteFindListQuery(KiggDbContext context, Expression<Func<Vote, bool>> predicate)
            : base(context, predicate)
        {
        }

        public override IEnumerable<Vote> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
