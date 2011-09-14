namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System.Linq;
    using System.Diagnostics;
    
    using DomainObjects;

    public class QueryFactory : IQueryFactory
    {
        private KiggDbContext dbContext;

        private KiggDbContext DbContext
        {
            [DebuggerStepThrough]
            get
            {
                return dbContext ?? (dbContext = dbContextFactory.Get());
            }
        }

        public bool UseCompiled { get; private set; }

        private readonly IKiggDbFactory dbContextFactory;

        public QueryFactory(IKiggDbFactory dbContextFactory, bool useCompiled)
        {
            Check.Argument.IsNotNull(dbContextFactory, "dbContextFactory");
            UseCompiled = useCompiled;
            this.dbContextFactory = dbContextFactory;
        }

        public IQuery<Tag> CreateFindTagByUniqueName(string uniqueName)
        {
            var query = new TagFindByUniqueKeyQuery(DbContext, t => t.UniqueName == uniqueName);
            
            return query;
        }

        public IQuery<Tag> CreateFindTagByName(string name)
        {
            var query = new TagFindByUniqueKeyQuery(DbContext, t => t.Name == name);

            return query;
        }

        public IOrderedQuery<Tag> CreateFindTagsByMatchingName(string name, int max)
        {
            IOrderedQuery<Tag> query = new TagFindListQuery(DbContext, t => t.Name.StartsWith(name));
            query = query.OrderBy(t => t.Name).Limit(max);
            return query;
        }

        public IOrderedQuery<Tag> CreateFindTagsByUsage(int max)
        {
            IOrderedQuery<Tag> query = new TagFindListQuery(DbContext, t =>t.Stories.Any());
            query = query.OrderByDescending(t => t.Stories.Count(st => st.ApprovedAt != null))
                         .ThenBy(t => t.Name)
                         .Limit(max);

            return query;
        }
    }
}
