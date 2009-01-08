namespace Kigg.Infrastructure
{
    public class CachingContentService : DecoratedContentService
    {
        private readonly ICache _cache;
        private readonly float _contentCacheDurationInMinutes;
        private readonly float _urlCacheDurationInMinutes;

        public CachingContentService(IContentService innerService, ICache cache, float contentCacheDurationInMinutes, float urlCacheDurationInMinutes) : base(innerService)
        {
            Check.Argument.IsNotNull(cache, "cache");
            Check.Argument.IsNotNegativeOrZero(contentCacheDurationInMinutes, "contentCacheDurationInMinutes");
            Check.Argument.IsNotNegativeOrZero(urlCacheDurationInMinutes, "urlCacheDurationInMinutes");

            _cache = cache;
            _contentCacheDurationInMinutes = contentCacheDurationInMinutes;
            _urlCacheDurationInMinutes = urlCacheDurationInMinutes;
        }

        public override StoryContent Get(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string cacheKey = "content:{0}".FormatWith(url);

            StoryContent result;

            _cache.TryGet(cacheKey, out result);

            if ((result == null) || (result == StoryContent.Empty))
            {
                result = base.Get(url);

                if ((result != StoryContent.Empty) && !_cache.Contains(cacheKey))
                {
                    _cache.Set(cacheKey, result, SystemTime.Now().AddMinutes(_contentCacheDurationInMinutes));
                }
            }

            return result;
        }

        public override string ShortUrl(string url)
        {
            Check.Argument.IsNotEmpty(url, "url");

            string cacheKey = "shortUrl:{0}".FormatWith(url);

            string shortUrl;

            _cache.TryGet(cacheKey, out shortUrl);

            if (string.IsNullOrEmpty(shortUrl))
            {
                shortUrl = base.ShortUrl(url);

                if ((!string.IsNullOrEmpty(shortUrl)) && (!_cache.Contains(cacheKey)))
                {
                    _cache.Set(cacheKey, shortUrl, SystemTime.Now().AddMinutes(_urlCacheDurationInMinutes));
                }
            }

            return shortUrl;
        }
    }
}