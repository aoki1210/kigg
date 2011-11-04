namespace Kigg.Repository
{
    using System;
    using System.Diagnostics;

    using Domain.Entities;

    public abstract class DecoratedStoryRepository : IStoryRepository
    {
        private readonly IStoryRepository _innerRepository;

        protected DecoratedStoryRepository(IStoryRepository innerRepository)
        {
            Check.Argument.IsNotNull(innerRepository, "innerRepository");

            _innerRepository = innerRepository;
        }

        [DebuggerStepThrough]
        public virtual void Add(Story entity)
        {
            _innerRepository.Add(entity);
        }

        [DebuggerStepThrough]
        public virtual void Remove(Story entity)
        {
            _innerRepository.Remove(entity);
        }

        [DebuggerStepThrough]
        public virtual Story FindById(long id)
        {
            return _innerRepository.FindById(id);
        }

        [DebuggerStepThrough]
        public virtual Story FindByUniqueName(string uniqueName)
        {
            return _innerRepository.FindByUniqueName(uniqueName);
        }

        [DebuggerStepThrough]
        public virtual Story FindByUrl(string url)
        {
            return _innerRepository.FindByUrl(url);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPublished(int start, int max)
        {
            return _innerRepository.FindPublished(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPublishedByCategory(long categoryId, int start, int max)
        {
            return _innerRepository.FindPublishedByCategory(categoryId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPublishedByCategory(string category, int start, int max)
        {
            return _innerRepository.FindPublishedByCategory(category, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindUpcoming(int start, int max)
        {
            return _innerRepository.FindUpcoming(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindNew(int start, int max)
        {
            return _innerRepository.FindNew(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindUnapproved(int start, int max)
        {
            return _innerRepository.FindUnapproved(start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            return _innerRepository.FindPublishable(minimumDate, maximumDate, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindByTag(long tagId, int start, int max)
        {
            //Check.Argument.IsNotEmpty(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            return _innerRepository.FindByTag(tagId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindByTag(string tag, int start, int max)
        {
            return _innerRepository.FindByTag(tag, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPostedByUser(long userId, int start, int max)
        {
            return _innerRepository.FindPostedByUser(userId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPostedByUser(string userName, int start, int max)
        {
            return _innerRepository.FindPostedByUser(userName, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPromotedByUser(long userId, int start, int max)
        {
            return _innerRepository.FindPromotedByUser(userId, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindPromotedByUser(string userName, int start, int max)
        {
            return _innerRepository.FindPromotedByUser(userName, start, max);
        }

        [DebuggerStepThrough]
        public virtual PagedResult<Story> FindCommentedByUser(long userId, int start, int max)
        {
            return _innerRepository.FindCommentedByUser(userId, start, max);
        }

        [DebuggerStepThrough]
        public virtual int CountPublished()
        {
            return _innerRepository.CountPublished();
        }

        [DebuggerStepThrough]
        public virtual int CountUpcoming()
        {
            return _innerRepository.CountUpcoming();
        }

        [DebuggerStepThrough]
        public virtual int CountByCategory(long categoryId)
        {
            return _innerRepository.CountByCategory(categoryId);
        }

        [DebuggerStepThrough]
        public virtual int CountByTag(long tagId)
        {
            return _innerRepository.CountByTag(tagId);
        }

        [DebuggerStepThrough]
        public virtual int CountNew()
        {
            return _innerRepository.CountNew();
        }

        [DebuggerStepThrough]
        public virtual int CountUnapproved()
        {
            return _innerRepository.CountUnapproved();
        }

        [DebuggerStepThrough]
        public virtual int CountPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            return _innerRepository.CountPublishable(minimumDate, maximumDate);
        }

        [DebuggerStepThrough]
        public virtual int CountPostedByUser(long userId)
        {
            return _innerRepository.CountPostedByUser(userId);
        }

    }
}