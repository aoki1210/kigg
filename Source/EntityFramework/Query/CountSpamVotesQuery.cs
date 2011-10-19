namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    
    using DomainObjects;
    
    public class CountSpamVotesQuery : QueryBase<int>
    {
        private readonly Expression<Func<SpamVote, bool>> predicate;

        public CountSpamVotesQuery(KiggDbContext context, Expression<Func<SpamVote, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override int Execute()
        {
            return context.Spams.Count(predicate);
        }
    }
}
