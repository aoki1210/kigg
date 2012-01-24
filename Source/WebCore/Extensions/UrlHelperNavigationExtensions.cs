namespace Kigg.Web
{
    using System.Web.Mvc;

    public static class UrlHelperNavigationExtensions
    {
        public static string ApplicationRoot(this UrlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.RequestContext.HttpContext.ApplicationRoot();
        }

        public static string Home(this UrlHelper instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return instance.Content(Constants.RouteNames.Root);
        }

        public static string Xrds(this UrlHelper instance)
        {
            return RouteUrl(instance, Constants.RouteNames.Xdrs, null);
        }

        public static string OpenId(this UrlHelper instance)
        {
            return RouteUrl(instance, Constants.RouteNames.OpenId, null);
        }

        public static string Logout(this UrlHelper instance)
        {
            return RouteUrl(instance, Constants.RouteNames.Logout, new { returnUrl = instance.Home() });
        }

        public static string ToAbsolute(this UrlHelper instance, string relativeUrl)
        {
            Check.Argument.IsNotNull(instance, "instance");
            Check.Argument.IsNotNullOrEmpty(relativeUrl, "relativeUrl");

            return instance.RequestContext.HttpContext.ApplicationRoot() + relativeUrl;
        }

        private static string RouteUrl(UrlHelper instance, string routeName, object routeValues)
        {
            Check.Argument.IsNotNull(instance, "instance");

            return (routeValues == null) ? instance.RouteUrl(routeName) : instance.RouteUrl(routeName, routeValues);
        }
    }
}