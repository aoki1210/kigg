namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class UserFindListQuery : OrderedQueryBase<User, IEnumerable<User>>
    {
        private IQueryable<User> query;

        public UserFindListQuery(KiggDbContext context)
            : base(context)
        {
            query = base.context.Users;
        }

        public UserFindListQuery(KiggDbContext context, Expression<Func<User, bool>> predicate)
            : base(context)
        {
            query = base.context.Users.Where(predicate);
        }

        public override IEnumerable<User> Execute()
        {
            return query.AsEnumerable();
        }

        public override IQuery<IEnumerable<User>> OrderBy<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            query = query.OrderBy(orderBy);
            return this;
        }

        public override IQuery<IEnumerable<User>> OrderByDescending<TKey>(Expression<Func<User, TKey>> orderBy)
        {
            query = query.OrderByDescending(orderBy);
            return this;
        }
    }
}
