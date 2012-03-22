namespace Kigg.Repository
{
    using System.Collections.Generic;

    using Domain.Entities;
    using Infrastructure;

    public class CachingCategoryRepository : DecoratedCategoryRepository
    {
        private readonly float _cacheDurationInMinutes;

        public CachingCategoryRepository(ICategoryRepository innerRepository, float cacheDurationInMinutes) : base(innerRepository)
        {
            Check.Argument.IsNotZeroOrNegative(cacheDurationInMinutes, "cacheDurationInMinutes");

            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public override IEnumerable<Category> FindAll()
        {
            const string CacheKey = "categories:All";

            IEnumerable<Category> result;

            Cache.TryGet(CacheKey, out result);

            if (result == null)
            {
                result = base.FindAll();

                if ((!result.IsNullOrEmpty()) && (!Cache.Contains(CacheKey)))
                {
                    Cache.Set(CacheKey, result, SystemTime.Now.AddMinutes(_cacheDurationInMinutes));
                }
            }

            return result;
        }
    }
}