namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class UserFindUniqueQuery : QueryBase<User>
    {
        private readonly Expression<Func<User, bool>> predicate;

        public UserFindUniqueQuery(KiggDbContext context, Expression<Func<User, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override User Execute()
        {
            return context.Users.Single(predicate);
        }
    }
}
