namespace Kigg.Repository
{
    using Domain.Entities;

    public interface IUniqueNameEntityRepository<TEntity> : IEntityRepository<TEntity> 
        where TEntity : class, IUniqueNameEntity
    {
        TEntity FindByUniqueName(string uniqueName);
    }
}