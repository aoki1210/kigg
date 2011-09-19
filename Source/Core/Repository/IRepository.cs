namespace Kigg.Repository
{
    public interface IRepository<TEntity>
    {
        TEntity FindById(long id);

        void Add(TEntity entity);

        void Remove(TEntity entity);
    }
}