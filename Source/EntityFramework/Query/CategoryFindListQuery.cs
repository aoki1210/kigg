namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class CategoryFindListQuery : OrderedQueryBase<Category>
    {
        public CategoryFindListQuery(KiggDbContext context) : base(context)
        {
            Query = base.context.Categories;
        }

        public CategoryFindListQuery(KiggDbContext context, Expression<Func<Category, bool>> predicate): base(context)
        {
            Query = base.context.Categories.Where(predicate);
        }

        public override IEnumerable<Category> Execute()
        {
            return Query.AsEnumerable();
        }
    }
}
