namespace Kigg.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RequestContextExtensions
    {
        public static UrlHelper UrlHelper(this RequestContext instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return new UrlHelper(instance);
        }
    }
}