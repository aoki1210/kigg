namespace Kigg.EF
{
    using System;
    using System.Threading;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Objects.DataClasses;

    using Kigg.DomainObjects;

    public class EntityCollection<TInterface, TEntity> : IEntityCollection<TInterface>
        where TInterface : class, IEntity
        where TEntity : class, IEntityWithRelationships, TInterface
    {
        private readonly EntityCollection<TEntity> _entityCollection;
        
        private bool _isLoaded;
        private object _syncRoot;

        public EntityCollection(EntityCollection<TEntity> entityCollection)
        {
            _isLoaded = entityCollection.IsLoaded;
            _entityCollection = entityCollection;
        }

        public int Count { get { return _entityCollection.Count; } }
        public bool IsReadOnly { get { return _entityCollection.IsReadOnly; } }
        public virtual bool IsLoaded
        {
            get { return _entityCollection.IsLoaded || _isLoaded; }
        }
        public virtual void Load()
        {
            try
            {
                _entityCollection.Load();
            }
            catch (InvalidOperationException)
            {
                //An exception will thrown in case Object EntityState is Detached or Deleted

                _isLoaded = true;
            }
        }
        public virtual IQueryable<TEntity> CreateSourceQuery()
        {
            var query = _entityCollection.CreateSourceQuery();
            return query ?? _entityCollection.AsQueryable();
        }

        public bool Contains(TInterface entity)
        {
            return _entityCollection.Contains(entity as TEntity);
        }
        public void Add(TInterface entity)
        {
            _entityCollection.Add(entity as TEntity);
        }
        public bool Remove(TInterface entity)
        {
            return _entityCollection.Remove(entity as TEntity);
        }
        public void Clear()
        {
            _entityCollection.Clear();
        }
        
        public void CopyTo(Array array, int arrayIndex)
        {
            var entitiesArray = array.Cast<TEntity>().ToArray();
            _entityCollection.CopyTo(entitiesArray, arrayIndex);
            entitiesArray.CopyTo(array, arrayIndex);
        }
        public IEnumerator<TInterface> GetEnumerator()
        {
            foreach (var entity in _entityCollection)
            {
                yield return entity;
            }
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ICollection<TEntity> Members

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return _syncRoot;

            }
        }

        #endregion

        #region IEnumerable<TInterface> Members

        IEnumerator<TInterface> IEnumerable<TInterface>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
