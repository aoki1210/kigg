namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class TagFindListQuery : OrderedQueryBase<Tag>
    {
        public TagFindListQuery(KiggDbContext context) : base(context)
        {
            Query = base.context.Tags;
        }

        public TagFindListQuery(KiggDbContext context, Expression<Func<Tag, bool>> predicate): base(context)
        {
            Query = base.context.Tags.Where(predicate);
        }

        public override IEnumerable<Tag> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
