namespace Kigg.Infrastructure.EntityFramework
{
    using System.Linq;
    
    using Query;
    using DomainObjects;
    using Repository;

    public abstract class EntityRepositoryBase<TEntity> : DomainObjectRepositoryBase<TEntity>, IEntityRepository<TEntity> 
        where TEntity : class, IEntity
    {
        protected EntityRepositoryBase(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public virtual TEntity FindById(long id)
        {
            Check.Argument.IsNotNegativeOrZero(id, "id");
            return DbContext.Set<TEntity>().SingleOrDefault(entity => entity.Id == id);
        }
        
    }
}
