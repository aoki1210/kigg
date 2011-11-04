namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using Domain.Entities;
    using Repository;
    using Query;
    
    public class TagRepository : EntityRepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public override void Add(Tag entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            if (Exists(t => t.Name == entity.Name))
            {
                throw new ArgumentException("\"{0}\" tag already exits. Specifiy a diffrent name.".FormatWith(entity.Name), "entity");
            }

            entity.UniqueName = UniqueNameGenerator.GenerateFrom(DbContext.Tags, entity.Name);

            base.Add(entity);
        }

        public Tag FindByName(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");

            var query = QueryFactory.CreateFindUniqueTagByName(name);
            
            return query.Execute();
        }

        public Tag FindByUniqueName(string uniqueName)
        {
            var query = QueryFactory.CreateFindUniqueTagByUniqueName(uniqueName);

            return query.Execute();
        }

        public IEnumerable<Tag> FindByUsage(int max)
        {
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindTagsByUsage(max);

            return query.Execute();
        }

        public IEnumerable<Tag> FindMatching(string name, int max)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindTagsByMatchingName(name, max);

            return query.Execute();
        }

        public IEnumerable<Tag> FindAll()
        {
            var query = QueryFactory.CreateFindAllTags(t => t.Name);
            return query.Execute();
        }
    }
}
