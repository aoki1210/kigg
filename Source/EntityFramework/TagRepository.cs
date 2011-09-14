

namespace Kigg.Infrastructure.EntityFramework
{
    using System.Collections.Generic;
    using DomainObjects;
    using Repository;
    using Query;
    
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public Tag FindByName(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");

            var query = QueryFactory.CreateFindTagByName(name);
            
            return query.Execute();
        }

        public IEnumerable<Tag> FindByUsage(int max)
        {
            Check.Argument.IsNotNegativeOrZero(max, "max");

            var query = QueryFactory.CreateFindTagsByUsage(max);

            return query.Execute();
        }

        public IEnumerable<Tag> FindMatching(string name, int max)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");
            Check.Argument.IsNotNegativeOrZero(max, "max");

            var query = QueryFactory.CreateFindTagsByMatchingName(name, max);

            return query.Execute();
        }

        public IEnumerable<Tag> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Tag FindById(long id)
        {
            throw new System.NotImplementedException();
        }

        public Tag FindByUniqueName(string uniqueName)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(Tag entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
