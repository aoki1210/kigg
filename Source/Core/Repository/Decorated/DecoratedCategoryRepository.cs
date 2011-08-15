namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using DomainObjects;

    public abstract class DecoratedCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryRepository _innerRepository;

        protected DecoratedCategoryRepository(ICategoryRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        [DebuggerStepThrough]
        public virtual void Add(Category entity)
        {
            _innerRepository.Add(entity);
        }

        [DebuggerStepThrough]
        public virtual void Remove(Category entity)
        {
            _innerRepository.Remove(entity);
        }

        [DebuggerStepThrough]
        public virtual Category FindById(long id)
        {
            return _innerRepository.FindById(id);
        }

        [DebuggerStepThrough]
        public virtual Category FindByUniqueName(string uniqueName)
        {
            return _innerRepository.FindByUniqueName(uniqueName);
        }

        [DebuggerStepThrough]
        public virtual ICollection<Category> FindAll()
        {
            return _innerRepository.FindAll();
        }
    }
}