namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Diagnostics;
    
    using Query;
    using Domain.Entities;
    using Repository;

    public abstract class DomainObjectRepositoryBase<TDomainObject> : IRepository<TDomainObject>
        where TDomainObject : class, IDomainObject
    {
        private KiggDbContext dbContext;

        protected DomainObjectRepositoryBase(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
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

        public virtual void Add(TDomainObject entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            DbContext.Add(entity);
        }

        public virtual void Remove(TDomainObject entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            DbContext.Remove(entity);
        }

        protected PagedResult<TDomainObject> CreatePagedResult(IOrderedQuery<TDomainObject> query)
        {
            int total = query.Count();

            return new PagedResult<TDomainObject>(query.Execute(), total);
        }

        protected bool Exists(Expression<Func<TDomainObject,bool>> predicate)
        {
            return DbContext.Set<TDomainObject>().Any(predicate);
        }
    }
}