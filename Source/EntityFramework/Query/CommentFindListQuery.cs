namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;
    public class CommentFindListQuery : OrderedQueryBase<Comment>
    {
        public CommentFindListQuery(KiggDbContext context) : base(context)
        {
        }

        public CommentFindListQuery(KiggDbContext context, Expression<Func<Comment, bool>> predicate)
            : base(context, predicate)
        {
        }

        public override IEnumerable<Comment> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
