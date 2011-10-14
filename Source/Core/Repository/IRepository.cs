namespace Kigg.Repository
{
    using DomainObjects;

    public interface IRepository<TDomainObject> where TDomainObject: class, IDomainObject
    {
        void Add(TDomainObject entity);

        void Remove(TDomainObject entity);
    }

    public interface IEntityRepository<TEntity> : IRepository<TEntity> 
        where TEntity: class, IEntity
    {
        TEntity FindById(long id);
    }
}