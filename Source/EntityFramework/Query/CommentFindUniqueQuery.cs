namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class CommentFindUniqueQuery : QueryBase<Comment>
    {
        private readonly Expression<Func<Comment, bool>> predicate;

        public CommentFindUniqueQuery(KiggDbContext context, Expression<Func<Comment, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override Comment Execute()
        {
            return context.Comments.Single(predicate);
        }
    }
}
