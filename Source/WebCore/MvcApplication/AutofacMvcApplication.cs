namespace Kigg.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using MvcExtensions;
    using AutofacApplication = MvcExtensions.Autofac.AutofacMvcApplication;

    using BoostrapperTasks;

    public class AutofacMvcApplication : AutofacApplication
    {
        public AutofacMvcApplication()
        {
            Bootstrapper.BootstrapperTasks
                .Include<RegisterViewEngines>()
                .Include<RegisterControllers>()
                .Include<AutoMapperBootstrapper>()
                //.Include<ConfigureAssets>()
                .Include<CreateDefaultUsers>()
                .Include<CreateDefaultCategories>();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Default", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected override void OnStart()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            base.OnStart();
        }
    }
}
