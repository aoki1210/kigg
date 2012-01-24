namespace Kigg.Web.BoostrapperTasks
{
    using MvcExtensions;
    using Controllers;

    public class ConfigureFilters : ConfigureFiltersBase
    {
        public ConfigureFilters(IFilterRegistry registry)
            : base(registry)
        {
        }

        protected override void Configure()
        {
            //Registry.Register<GeneralController, CompressAttribute>(c => c.JavaScriptConstants());
            //.Register<ElmahHandleErrorAttribute>(new TypeCatalogBuilder().Add(GetType().Assembly).Include(type => typeof(Controller).IsAssignableFrom(type)));
        }
    }
}
