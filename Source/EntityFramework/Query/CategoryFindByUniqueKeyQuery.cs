namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class CategoryFindByUniqueKeyQuery : QueryBase<Category>
    {
        private static readonly Expression<Func<KiggDbContext, Expression<Func<Category, bool>>, Category>> expression = (ctx, predicate) => ctx.Categories.Single(predicate);
        private static readonly Func<KiggDbContext, Expression<Func<Category, bool>>, Category> plainQuery = expression.Compile();

        private readonly Expression<Func<Category, bool>> predicate;

        public CategoryFindByUniqueKeyQuery(Expression<Func<Category, bool>> predicate)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override Category Execute(KiggDbContext context)
        {
            return plainQuery(context, predicate);
        }
    }
}
