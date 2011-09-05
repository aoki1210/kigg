namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class UserFindByUniqueKeyQuery : QueryBase<User>
    {
        private readonly Expression<Func<User, bool>> predicate;

        public UserFindByUniqueKeyQuery(KiggDbContext context, Expression<Func<User, bool>> predicate)
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
