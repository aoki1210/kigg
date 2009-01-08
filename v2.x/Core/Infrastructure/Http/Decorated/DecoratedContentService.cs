namespace Kigg.Infrastructure
{
    public abstract class DecoratedContentService : IContentService
    {
        private readonly IContentService _innerService;

        protected DecoratedContentService(IContentService innerService)
        {
            Check.Argument.IsNotNull(innerService, "innerService");

            _innerService = innerService;
        }

        public virtual StoryContent Get(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, url);

            return _innerService.Get(url);
        }

        public virtual void Ping(string url, string title, string fromUrl, string excerpt, string siteTitle)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");
            Check.Argument.IsNotEmpty(title, "title");
            Check.Argument.IsNotEmpty(fromUrl, "fromUrl");
            Check.Argument.IsNotEmpty(excerpt, "exceprt");
            Check.Argument.IsNotEmpty(siteTitle, "siteTitle");

            _innerService.Ping(url, title, fromUrl, excerpt, siteTitle);
        }

        public virtual string ShortUrl(string url)
        {
            Check.Argument.IsNotEmpty(url, "url");

            return _innerService.ShortUrl(url);
        }
    }
}