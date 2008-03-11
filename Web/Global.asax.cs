namespace Kigg
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Routing;
    using System.Web.Mvc;
    using System.Web.Security;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            CreateDefaultUserIfNotExists();
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            int iisVersion = Convert.ToInt32(ConfigurationManager.AppSettings["IISVersion"], System.Globalization.CultureInfo.InvariantCulture);

            if (iisVersion >= 7)
            {
                RegisterRoutesForNewIIS(routes);
            }
            else
            {
                RegisterRoutesForOldIIS(routes);
            }
        }

        private static void RegisterRoutesForNewIIS(ICollection<RouteBase> routes)
        {
            routes.Add(new Route("User/Login", new RouteValueDictionary(new { controller = "User", action = "Login" }), new MvcRouteHandler()));
            routes.Add(new Route("User/Logout", new RouteValueDictionary(new { controller = "User", action = "Logout" }), new MvcRouteHandler()));
            routes.Add(new Route("User/Signup", new RouteValueDictionary(new { controller = "User", action = "Signup" }), new MvcRouteHandler()));
            routes.Add(new Route("User/SendPassword", new RouteValueDictionary(new { controller = "User", action = "SendPassword" }), new MvcRouteHandler()));

            routes.Add(new Route("Story/Detail/{id}", new RouteValueDictionary(new { controller = "Story", action = "Detail" }), new MvcRouteHandler()));
            routes.Add(new Route("Story/Upcoming/{page}", new RouteValueDictionary(new { controller = "Story", action = "Upcoming" }), new MvcRouteHandler()));
            routes.Add(new Route("Story/Search/{q}/{page}", new RouteValueDictionary(new { controller = "Story", action = "Search" }), new MvcRouteHandler()));

            var defaults = new RouteValueDictionary (
                                                        new
                                                        {
                                                            controller = "Story",
                                                            action = "Category",
                                                            name = (string)null,
                                                            page = (int?)null
                                                        }
                                                    );

            routes.Add(new Route("Story/Category/{page}", defaults, new MvcRouteHandler()));
            routes.Add(new Route("Story/{action}/{name}/{page}", defaults, new MvcRouteHandler()));
            routes.Add(new Route("{controller}/{action}/{id}", defaults, new MvcRouteHandler()));
            routes.Add(new Route("Default.aspx", defaults, new MvcRouteHandler()));
        }

        private static void RegisterRoutesForOldIIS(ICollection<RouteBase> routes)
        {
            routes.Add(new Route("User.mvc/Login", new RouteValueDictionary(new { controller = "User", action = "Login" }), new MvcRouteHandler()));
            routes.Add(new Route("User.mvc/Logout", new RouteValueDictionary(new { controller = "User", action = "Logout" }), new MvcRouteHandler()));
            routes.Add(new Route("User.mvc/Signup", new RouteValueDictionary(new { controller = "User", action = "Signup" }), new MvcRouteHandler()));
            routes.Add(new Route("User.mvc/SendPassword", new RouteValueDictionary(new { controller = "User", action = "SendPassword" }), new MvcRouteHandler()));

            routes.Add(new Route("Story.mvc/Detail/{id}", new RouteValueDictionary(new { controller = "Story", action = "Detail" }), new MvcRouteHandler()));
            routes.Add(new Route("Story.mvc/Upcoming/{page}", new RouteValueDictionary(new { controller = "Story", action = "Upcoming" }), new MvcRouteHandler()));
            routes.Add(new Route("Story.mvc/Search/{q}/{page}", new RouteValueDictionary(new { controller = "Story", action = "Search" }), new MvcRouteHandler()));

            var defaults = new RouteValueDictionary(
                                                        new
                                                        {
                                                            controller = "Story",
                                                            action = "Category",
                                                            name = (string)null,
                                                            page = (int?)null
                                                        }
                                                    );

            routes.Add(new Route("Story.mvc/Category/{page}", defaults, new MvcRouteHandler()));
            routes.Add(new Route("Story.mvc/{action}/{name}/{page}", defaults, new MvcRouteHandler()));
            routes.Add(new Route("{controller}.mvc/{action}/{id}", defaults, new MvcRouteHandler()));
            routes.Add(new Route("Default.aspx", defaults, new MvcRouteHandler()));
        }

        private static void CreateDefaultUserIfNotExists()
        {
            const string DefaultUser = "admin";
            const string DefaultPassword = "admin";
            const string DefaultEmail = "admin@kigg.com";

            if (Membership.GetUser(DefaultUser) != null) return;

            Membership.CreateUser(DefaultUser, DefaultPassword, DefaultEmail);
        }
    }
}