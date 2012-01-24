namespace Kigg.Web.BoostrapperTasks
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using MvcExtensions;

    public class RegisterRoutes : RegisterRoutesBase
    {
        public RegisterRoutes(RouteCollection routes)
            : base(routes)
        {
        }

        protected override void Register()
        {
            Routes.Clear();

            // Turns off the unnecessary file exists check
            Routes.RouteExistingFiles = true;

            // Ignore axd files
            Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignore known static files
            Routes.IgnoreRoute("{file}.txt");
            Routes.IgnoreRoute("{file}.htm");
            Routes.IgnoreRoute("{file}.html");
            Routes.IgnoreRoute("{file}.xml");

            // Ignore the assets directory which contains css, images and js
            Routes.IgnoreRoute("Content/{*pathInfo}");
            Routes.IgnoreRoute("Scripts/{*pathInfo}");

            Routes.MapRoute(Constants.RouteNames.JsonConstants, "json-constants", new { controller = "General", action = "JavaScriptConstants" });
            
            Routes.MapRoute(Constants.RouteNames.Xdrs, "Xrds", new { controller = "Membership", action = "Xrds" });
            Routes.MapRoute(Constants.RouteNames.OpenId, "OpenId", new { controller = "Membership", action = "OpenId" });
            Routes.MapRoute(Constants.RouteNames.Logout, "Logout", new { controller = "Membership", action = "Logout" });

            Routes.MapRoute(Constants.RouteNames.Login, "Login", new { controller = "Home", action = "Login" });
            Routes.MapRoute(Constants.RouteNames.Published, "{page}", new { controller = "Home", action = "Default", page = 1 }, new { page = @"^\d+$" });
            Routes.MapRoute(Constants.RouteNames.Default, "{controller}/{action}", new { controller = "Home", action = "Default" });
            
        }

    }
}
