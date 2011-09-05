﻿namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using DomainObjects;

    public class CategoryFindByUniqueKeyQuery : QueryBase<Category>
    {
        private readonly Expression<Func<Category, bool>> predicate;

        public CategoryFindByUniqueKeyQuery(KiggDbContext context, Expression<Func<Category, bool>> predicate)
            : base(context)
        {
            Check.Argument.IsNotNull(predicate, "predicate");

            this.predicate = predicate;
        }

        public override Category Execute()
        {
            return context.Categories.Single(predicate);
        }

    }
}
