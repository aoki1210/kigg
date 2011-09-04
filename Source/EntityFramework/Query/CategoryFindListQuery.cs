namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class CategoryFindListQuery : QueryBase<IEnumerable<Category>>
    {
        private static readonly Expression<Func<KiggDbContext, Expression<Func<Category, bool>>, IQueryable<Category>>> expressionWithCondition = (ctx, predicate) => ctx.Categories.Where(predicate);
        private static readonly Func<KiggDbContext, Expression<Func<Category, bool>>, IQueryable<Category>> plainQueryWithCondition = expressionWithCondition.Compile();

        private static readonly Expression<Func<KiggDbContext, IQueryable<Category>>> expression = (ctx) => ctx.Categories;
        private static readonly Func<KiggDbContext, IQueryable<Category>> plainQuery = expression.Compile();

        private readonly Expression<Func<Category, bool>> predicate;

        public CategoryFindListQuery()
            : this(null)
        {

        }

        public CategoryFindListQuery(Expression<Func<Category, bool>> predicate)
        {
            this.predicate = predicate;
        }

        public override IEnumerable<Category> Execute(KiggDbContext context)
        {
            return (predicate == null) ? plainQuery(context)
                                       : plainQueryWithCondition(context, predicate);
        }
    }
}
