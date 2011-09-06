﻿namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class UserFindTopByScoredQuery : OrderedQueryBase<User, IEnumerable<User>>
    {
        private readonly IQueryable<User> originalQuery;

        public UserFindTopByScoredQuery(KiggDbContext context, DateTime startTimestamp, DateTime endTimestamp)
            : base(context)
        {
            var userWithScore = context.Scores
                                        .Where(us => (us.ScoredBy.Role == Roles.User) && (!us.ScoredBy.IsLockedOut) && (us.CreatedAt >= startTimestamp && us.CreatedAt <= endTimestamp))
                                        .GroupBy(us => us.ScoredBy.Id)
                                        .Select(g => new { UserId = g.Key, Total = g.Sum(us => us.Score) });

            originalQuery = from user in context.Users
                    join score in userWithScore
                    on user.Id equals score.UserId
                    where score.Total > 0
                    orderby score.Total descending, user.LastActivityAt descending
                    select user;

            Query = originalQuery;
        }

        public override IEnumerable<User> Execute()
        {
            return Query.AsEnumerable();
        }

        public int Count()
        {
            return originalQuery.Count();
        }

        public override IOrderedQuery<User, IEnumerable<User>> OrderBy<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
        public override IOrderedQuery<User, IEnumerable<User>> ThenBy<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
        public override IOrderedQuery<User, IEnumerable<User>> OrderByDescending<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
        public override IOrderedQuery<User, IEnumerable<User>> ThenByDescending<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            throw new NotSupportedException();
        }
    }
}
