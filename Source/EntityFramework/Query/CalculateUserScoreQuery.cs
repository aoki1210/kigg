namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class CalculateUserScoreQuery : QueryBase<decimal>
    {
        private readonly Expression<Func<UserScore, bool>> predicate;

        public CalculateUserScoreQuery(KiggDbContext context, Expression<Func<UserScore, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override decimal Execute()
        {
            var hasScore = context.Scores.Any(predicate);

            decimal score = (hasScore)
                        ? context.Scores.Where(predicate).Sum(us => us.Score)
                        : 0m;

            return score;
        }
    }
}
