namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using DomainObjects;

    public class CategoryFindListQuery : OrderedQueryBase<Category, IEnumerable<Category>>
    {
        private IQueryable<Category> query;

        public CategoryFindListQuery(KiggDbContext context) : base(context)
        {
            query = base.context.Categories;
        }

        public CategoryFindListQuery(KiggDbContext context, Expression<Func<Category, bool>> predicate): base(context)
        {
            query = base.context.Categories.Where(predicate);
        }

        public override IEnumerable<Category> Execute()
        {
            return query.AsEnumerable();
        }

        public override IQuery<IEnumerable<Category>> OrderBy<TKey>(Expression<Func<Category, TKey>> orderBy)
        {
            query = query.OrderBy(orderBy);
            return this;
        }

        public override IQuery<IEnumerable<Category>> OrderByDescending<TKey>(Expression<Func<Category, TKey>> orderBy)
        {
            query = query.OrderByDescending(orderBy);
            return this;
        }
    }
}
