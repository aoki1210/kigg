namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class DomainObjectFindUniqueQuery<TResult> : QueryBase<TResult>
        where TResult: class, IDomainObject
    {
        private readonly Expression<Func<TResult, bool>> predicate;

        public DomainObjectFindUniqueQuery(KiggDbContext context, Expression<Func<TResult, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override TResult Execute()
        {
            return context.Set<TResult>().Single(predicate);
        }
    }
}
