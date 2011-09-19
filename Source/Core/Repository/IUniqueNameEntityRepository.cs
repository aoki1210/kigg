namespace Kigg.Repository
{
    using DomainObjects;

    public interface IUniqueNameEntityRepository<TEntity> : IRepository<TEntity> where TEntity : IUniqueNameEntity
    {
        TEntity FindByUniqueName(string uniqueName);
    }
}