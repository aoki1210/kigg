namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using DomainObjects;

    public abstract class DecoratedUserRepository : IUserRepository
    {
        private readonly IUserRepository _innerRepository;

        protected DecoratedUserRepository(IUserRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        [DebuggerStepThrough]
        public virtual void Add(User entity)
        {
            _innerRepository.Add(entity);
        }

        [DebuggerStepThrough]
        public virtual void Remove(User entity)
        {
            _innerRepository.Remove(entity);
        }

        [DebuggerStepThrough]
        public virtual User FindById(long id)
        {
            return _innerRepository.FindById(id);
        }

        [DebuggerStepThrough]
        public virtual User FindByUserName(string userName)
        {
            return _innerRepository.FindByUserName(userName);
        }

        [DebuggerStepThrough]
        public virtual User FindByEmail(string email)
        {
            return _innerRepository.FindByEmail(email);
        }

        [DebuggerStepThrough]
        public virtual decimal FindScoreById(long id, DateTime startTimestamp, DateTime endTimestamp)
        {
            return _innerRepository.FindScoreById(id, startTimestamp, endTimestamp);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<User> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            return _innerRepository.FindTop(startTimestamp, endTimestamp, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<User> FindAll(int start, int max)
        {
            return _innerRepository.FindAll(start, max);
        }

        [DebuggerStepThrough]
        public virtual ICollection<string> FindIPAddresses(long id)
        {
            //Check.Argument.IsNotEmpty(id, "id");

            return _innerRepository.FindIPAddresses(id);
        }
    }
}