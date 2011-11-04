namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    
    using Domain.Entities;
    
    public class CountCommentsQuery : QueryBase<int>
    {
        private readonly Expression<Func<Comment, bool>> predicate;

        public CountCommentsQuery(KiggDbContext context, Expression<Func<Comment, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override int Execute()
        {
            return context.Comments.Count(predicate);
        }
    }
}
