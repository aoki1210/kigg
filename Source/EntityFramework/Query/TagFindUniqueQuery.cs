namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class TagFindUniqueQuery : QueryBase<Tag>
    {
        private readonly Expression<Func<Tag, bool>> predicate;

        public TagFindUniqueQuery(KiggDbContext context, Expression<Func<Tag, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override Tag Execute()
        {
            return context.Tags.Single(predicate);
        }

    }
}
