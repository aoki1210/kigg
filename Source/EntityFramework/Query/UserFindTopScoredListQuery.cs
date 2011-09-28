namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class UserFindTopScoredListQuery : OrderedQueryBase<User>
    {
        public UserFindTopScoredListQuery(KiggDbContext context, Expression<Func<UserScore, bool>> predicate) 
            : base(context)
        {
            var userWithScore = context.Scores
                                        .Where(predicate)
                                        .GroupBy(us => us.ScoredBy.Id)
                                        .Select(g => new { UserId = g.Key, Total = g.Sum(us => us.Score) });

            OriginalQuery = from user in context.Users
                            join score in userWithScore
                            on user.Id equals score.UserId
                            where score.Total > 0
                            orderby score.Total descending, user.LastActivityAt descending
                            select user;

            Query = OriginalQuery;
        }

        public override IEnumerable<User> Execute()
        {
            return Query.AsEnumerable();
        }

        public override long CountAllRecords()
        {
            return OriginalQuery.Count();
        }

        public override IOrderedQuery<User> OrderBy<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
        public override IOrderedQuery<User> ThenBy<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
        public override IOrderedQuery<User> OrderByDescending<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
        public override IOrderedQuery<User> ThenByDescending<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
    }
}
