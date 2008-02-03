namespace Kigg
{
    using System;
    using System.Web.Security;
    using System.Web.Mvc;

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
            CreateDefaultUserIfNotExists();
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            var defaults = new
            {
                controller = "Story",
                action = "Category",
                name = (string)null,
                page = (int?)null
            };

            routes.Add(
                            new Route
                            {
                                Url = "User/Login",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "Login"
                                }
                            }
                        );
            routes.Add(
                            new Route
                            {
                                Url = "User.mvc/Login",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "Login"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "User/Logout",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "Logout"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "User.mvc/Logout",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "Logout"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "User/Signup",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "Signup"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "User.mvc/Signup",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "Signup"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "User/SendPassword",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "SendPassword"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "User.mvc/SendPassword",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "User",
                                    action = "SendPassword"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story/Detail/[id]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "Story",
                                    action = "Detail"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story.mvc/Detail/[id]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "Story",
                                    action = "Detail"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story/Upcoming/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "Story",
                                    action = "Upcoming"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story.mvc/Upcoming/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "Story",
                                    action = "Upcoming"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story/Search/[q]/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "Story",
                                    action = "Search"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story.mvc/Search/[q]/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = new
                                {
                                    controller = "Story",
                                    action = "Search"
                                }
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story/Category/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = defaults
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story.mvc/Category/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = defaults
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story/[action]/[name]/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = defaults
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Story.mvc/[action]/[name]/[page]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = defaults
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "[controller]/[action]/[id]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = defaults
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "[controller].mvc/[action]/[id]",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = defaults
                            }
                        );

            routes.Add(
                            new Route
                            {
                                Url = "Default.aspx",
                                RouteHandler = typeof(MvcRouteHandler),
                                Defaults = defaults
                            }
                        );
        }

        private static void CreateDefaultUserIfNotExists()
        {
            const string DefaultUser = "admin";
            const string DefaultPassword = "admin";
            const string DefaultEmail = "admin@kigg.com";

            if (Membership.GetUser(DefaultUser) == null)
            {
                Membership.CreateUser(DefaultUser, DefaultPassword, DefaultEmail);
            }
        }
    }
}