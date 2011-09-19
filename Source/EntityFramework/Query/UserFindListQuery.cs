namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class UserFindListQuery : OrderedQueryBase<User>
    {
        private readonly IQueryable<User> originalQuery;
        public UserFindListQuery(KiggDbContext context)
            : base(context)
        {
            Query = base.context.Users;
        }

        public UserFindListQuery(KiggDbContext context, Expression<Func<User, bool>> predicate)
            : base(context)
        {
            originalQuery = base.context.Users.Where(predicate);
            Query = originalQuery;
        }

        public override long Count()
        {
            return originalQuery.Count();
        }

        public override IEnumerable<User> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
