namespace Kigg.EF
{
    using System.Collections;
    using System.Collections.Generic;
    
    using Kigg.DomainObjects;

    public interface IEntityCollection<TEntity> : ICollection, IEnumerable<TEntity>
        where TEntity : class, IEntity
    {
        bool IsLoaded { get; }
        void Load();
        void Clear();
        bool Remove(TEntity entity);
    }
}
