namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;
    public class StoryViewFindListQuery : OrderedQueryBase<StoryView>
    {
        public StoryViewFindListQuery(KiggDbContext context) : base(context)
        {
            Query = base.context.Views;
        }

        public StoryViewFindListQuery(KiggDbContext context, Expression<Func<StoryView, bool>> predicate)
            : base(context, predicate)
        {
        }

        public override IEnumerable<StoryView> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
