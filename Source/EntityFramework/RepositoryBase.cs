using System;
using System.Collections.Generic;

namespace Kigg.Infrastructure.EntityFramework
{
    using System.Linq;
    using System.Diagnostics;
    
    using Query;
    using DomainObjects;
    using Repository;

    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> 
        where TEntity : class, IEntity, IDomainObject
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

            DbContext.Add(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            DbContext.Remove(entity);
        }

        public virtual TEntity FindById(long id)
        {
            Check.Argument.IsNotNegativeOrZero(id, "id");
            return DbContext.Set<TEntity>().SingleOrDefault(entity => entity.Id == id);
        }

        protected PagedResult<TEntity> CreatePagedResult(IOrderedQuery<TEntity> query)
        {
            int total = query.Count();

            return new PagedResult<TEntity>(query.Execute(), total);
        }
        
    }
}
