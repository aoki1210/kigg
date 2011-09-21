namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

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

        public IQuery<Category> CreateFindCategoryByUniqueName(string uniqueName)
        {
            var query = new CategoryFindByUniqueKeyQuery(DbContext, c => c.UniqueName == uniqueName);

            return query;
        }

        public IQuery<Category> CreateFindCategoryByName(string name)
        {
            var query = new CategoryFindByUniqueKeyQuery(DbContext, c => c.Name == name);

            return query;
        }

        public IOrderedQuery<Category> CreateFindAllCategories<TKey>(Expression<Func<Category, TKey>> orderBy)
        {
            IOrderedQuery<Category> query = new CategoryFindListQuery(DbContext);
            query = query.OrderBy(orderBy);
            return query;
        }

        public IQuery<KnownSource> CreateFindKnownSourceByUrl(string url)
        {
            var query = new KnownSourceFindByUrlQuery(DbContext, url);
            return query;
        }

        public IOrderedQuery<KnownSource> CreateFindAllKnownSources<TKey>(Expression<Func<KnownSource, TKey>> orderBy)
        {
            IOrderedQuery<KnownSource> query = new KnownSourceFindListQuery(DbContext);
            query = query.OrderBy(orderBy);
            return query;
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
            IOrderedQuery<Tag> query = new TagFindListQuery(DbContext, t => t.Stories.Any());
            query = query.OrderByDescending(t => t.Stories.Count(st => st.ApprovedAt != null))
                         .ThenBy(t => t.Name)
                         .Limit(max);

            return query;
        }

        public IOrderedQuery<Tag> CreateFindAllTags<TKey>(Expression<Func<Tag, TKey>> orderBy)
        {
            IOrderedQuery<Tag> query = new TagFindListQuery(DbContext);
            query = query.OrderBy(orderBy);
            return query;
        }
        
        public IQuery<User> CreateFindUserByEmail(string email)
        {
            var query = new UserFindByUniqueKeyQuery(DbContext, u => u.Email == email);

            return query;
        }

        public IQuery<User> CreateFindUserByUserName(string userName)
        {
            var query = new UserFindByUniqueKeyQuery(DbContext, u => u.UserName == userName);

            return query;
        }

        public IQuery<int> CreateCountVotesByStoryId(long id)
        {
            Check.Argument.IsNotNegativeOrZero(id, "id");
            var query = new CountVotesQuery(DbContext, v => v.StoryId == id);
            return query;
        }
        public IQuery<decimal> CreateCalculateUserScoreById(long id, DateTime startDate, DateTime endDate)
        {
            Check.Argument.IsNotNegativeOrZero(id, "id");
            Check.Argument.IsNotInFuture(startDate, "startDate");
            Check.Argument.IsNotInFuture(endDate, "endDate");

            var query = new CalculateUserScoreQuery(DbContext,
                                                        us =>
                                                        (us.ScoredBy.Id == id) &&
                                                        (us.CreatedAt >= startDate && us.CreatedAt <= endDate));
            return query;
        }

        public IOrderedQuery<User> CreateFindTopScoredUsers(DateTime startDate, DateTime endDate, int start, int max)
        {
            Check.Argument.IsNotInFuture(startDate, "startDate");
            Check.Argument.IsNotInFuture(endDate, "endDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var query = new UserFindTopScoredListQuery(DbContext, startDate, endDate);
            return query.Page(start, max);
        }
        
        public IOrderedQuery<User> CreateFindAllUsers<TKey>(int start, int max, Expression<Func<User, TKey>> orderBy)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var query = new UserFindListQuery(DbContext);
            return query.OrderBy(orderBy).Page(start, max);
        }
    }
}
