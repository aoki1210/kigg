namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    
    using Domain.Entities;
    
    public class CountVotesQuery : QueryBase<int>
    {
        private readonly Expression<Func<Vote, bool>> predicate;

        public CountVotesQuery(KiggDbContext context, Expression<Func<Vote, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override int Execute()
        {
            return context.Votes.Count(predicate);
        }
    }
}
