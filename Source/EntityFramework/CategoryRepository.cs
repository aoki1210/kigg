namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Repository;
    using Query;

    public class CategoryRepository : EntityRepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public override void Add(Category entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            if (Exists(c => c.Name == entity.Name))
            {
                throw new ArgumentException("\"{0}\" category already exits. Specifiy a diffrent name.".FormatWith(entity.Name), "entity");
            }

            entity.UniqueName = UniqueNameGenerator.GenerateFrom(DbContext.Categories, entity.Name);

            base.Add(entity);
        }

        public Category FindByName(string name)
        {
            var query = QueryFactory.CreateFindUniqueCategoryByName(name);

            return query.Execute();
        }

        public Category FindByUniqueName(string uniqueName)
        {
            var query = QueryFactory.CreateFindUniqueCategoryByUniqueName(uniqueName);

            return query.Execute();
        }

        public IEnumerable<Category> FindAll()
        {
            var query = QueryFactory.CreateFindAllCategories(c => c.Name);
            
            return query.Execute();
        }
    }
}