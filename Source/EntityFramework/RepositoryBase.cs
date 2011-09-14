namespace Kigg.Infrastructure.EntityFramework
{
    using System.Linq;
    using System.Diagnostics;
    
    using Query;
    using DomainObjects;

    public abstract class RepositoryBase<TEntity> where TEntity : class, IEntity
    {
        private KiggDbContext dbContext;

        protected RepositoryBase(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
        {
            Check.Argument.IsNotNull(dbContextFactory, "dbContextFactory");
            Check.Argument.IsNotNull(queryFactory, "queryFactory");

            DbContextFactory = dbContextFactory;
            QueryFactory = queryFactory;
        }

        protected IKiggDbFactory DbContextFactory
        {
            get;
            private set;
        }

        protected IQueryFactory QueryFactory
        {
            get;
            private set;
        }
        
        protected KiggDbContext DbContext
        {
            [DebuggerStepThrough]
            get
            {
                return dbContext ?? (dbContext = DbContextFactory.Get());
            }
        }

        public virtual void Add(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            DbContext.Set<TEntity>().Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            DbContext.Set<TEntity>().Remove(entity);
        }

        public virtual TEntity GetById(long id)
        {
            return DbContext.Set<TEntity>().SingleOrDefault(entity => entity.Id == id);
        }
    }
}
