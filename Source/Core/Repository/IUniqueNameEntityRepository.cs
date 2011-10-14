namespace Kigg.Repository
{
    using DomainObjects;

    public interface IUniqueNameEntityRepository<TEntity> : IEntityRepository<TEntity> 
        where TEntity : class, IUniqueNameEntity
    {
        TEntity FindByUniqueName(string uniqueName);
    }
}