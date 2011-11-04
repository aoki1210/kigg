namespace Kigg.Repository
{
    using System.Collections.Generic;

    using Domain.Entities;
    using Infrastructure;

    public class CachingTagRepository : DecoratedTagRepository
    {
        private readonly float _cacheDurationInMinutes;

        public CachingTagRepository(ITagRepository innerRepository, float cacheDurationInMinutes) : base(innerRepository)
        {
            Check.Argument.IsNotZeroOrNegative(cacheDurationInMinutes, "cacheDurationInMinutes");

            _cacheDurationInMinutes = cacheDurationInMinutes;
        }

        public override IEnumerable<Tag> FindByUsage(int top)
        {
            Check.Argument.IsNotZeroOrNegative(top, "top");

            string cacheKey = "tagsByUsage:{0}".FormatWith(top);

            IEnumerable<Tag> result;

            Cache.TryGet(cacheKey, out result);

            if (result == null)
            {
                result = base.FindByUsage(top);

                if ((!result.IsNullOrEmpty()) && (!Cache.Contains(cacheKey)))
                {
                    Cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_cacheDurationInMinutes));
                }
            }

            return result;
        }
    }
}