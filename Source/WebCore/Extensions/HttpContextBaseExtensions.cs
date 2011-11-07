namespace Kigg.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class HttpContextBaseExtensions
    {
        public static RequestContext RequestContext(this HttpContextBase instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            RouteData routeData = RouteTable.Routes.GetRouteData(instance) ?? new RouteData();
            var requestContext = new RequestContext(instance, routeData);

            return requestContext;
        }

        public static UrlHelper UrlHelper(this HttpContextBase instance)
        {
            return new UrlHelper(instance.RequestContext());
        }

        public static string ApplicationRoot(this HttpContextBase instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            string applicationRoot = instance.Request.Url.GetLeftPart(UriPartial.Authority); //+ instance.Request.ApplicationPath;

            // Remove the last /
            if (applicationRoot.EndsWith("/", StringComparison.Ordinal))
            {
                applicationRoot = applicationRoot.Substring(0, applicationRoot.Length - 1);
            }

            return applicationRoot;
        }
    }
}