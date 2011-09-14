namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class UserFindListQuery : OrderedQueryBase<User>
    {
        public UserFindListQuery(KiggDbContext context)
            : base(context)
        {
            Query = base.context.Users;
        }

        public UserFindListQuery(KiggDbContext context, Expression<Func<User, bool>> predicate)
            : base(context)
        {
            Query = base.context.Users.Where(predicate);
        }

        public override IEnumerable<User> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
