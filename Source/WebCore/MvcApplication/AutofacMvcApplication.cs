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
                .Include<RegisterModelBinders>()
                .Include<RegisterRoutes>()
                .Include<AutoMapperBootstrapper>()
                .Include<ConfigureFilters>()
                .Include<CreateDefaultUsers>()
                .Include<CreateDefaultCategories>();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        
        protected override void OnStart()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            

            base.OnStart();
        }
    }
}
