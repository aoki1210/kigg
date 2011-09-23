namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class VoteFindUniqueQuery : QueryBase<Vote>
    {
        private readonly Expression<Func<Vote, bool>> predicate;

        public VoteFindUniqueQuery(KiggDbContext context, Expression<Func<Vote, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override Vote Execute()
        {
            return context.Votes.Single(predicate);
        }
    }
}
