using System;

namespace Kigg.EF.DomainObjects
{
    using System.Data.Objects.DataClasses;

    public class EntityReference<TInterface, TEntity> : IEntityReference<TInterface>
        where TInterface : class
        where TEntity : class, IEntityWithRelationships, TInterface
    {
        private readonly EntityReference<TEntity> _entityReference;
        private bool _isLoaded;
        public EntityReference(EntityReference<TEntity> entityReference)
        {
            _entityReference = entityReference;
            _isLoaded = entityReference.IsLoaded;
        }

#if(DEBUG)
        public virtual TInterface Value
#else
        public TInterface Value
#endif
        {
            get
            {
                return _entityReference.Value;
            }
        }

#if(DEBUG)
        public virtual bool IsLoaded
#else
        public bool IsLoaded
#endif
        {
            get { return _entityReference.IsLoaded || _isLoaded; }
        }

#if(DEBUG)
        public virtual void Load()
#else
        public void Load()
#endif
        {
            try
            {
                _entityReference.Load();
            }
            catch (InvalidOperationException)
            {
                //An exception will thrown in case Object EntityState is Detached or Deleted

                _isLoaded = true;
            }
        }

        public void Attach(TInterface entity)
        {
            _entityReference.Attach(entity as TEntity);
        }
    }
}
