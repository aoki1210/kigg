namespace Kigg.Web
{
    using System.Web;
    using System.Configuration;
    using System.Data.Common;
    
    using Autofac;

    using Caching;
    using Infrastructure;
    using Infrastructure.EntityFramework;
    using Infrastructure.EntityFramework.Query;
    using UnitOfWork = Infrastructure.EntityFramework.UnitOfWork;

    public class AutofacServiceRegistration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.Register(c => new CacheManager(HttpRuntime.Cache)).As<ICacheManager>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            RegisterRepositories(builder);
            
            base.Load(builder);
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DomainObjectRepositoryBase<>).Assembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["Kigg"];

            string providerName = connectionStringSettings.ProviderName;
            string connectionString = connectionStringSettings.ConnectionString;
            
            DbProviderFactory databaseProviderFactory = DbProviderFactories.GetFactory(providerName);
            
            builder.RegisterInstance(databaseProviderFactory);
            builder.Register(c => new KiggDbFactory(c.Resolve<DbProviderFactory>(), connectionString)).As<IKiggDbFactory>().InstancePerLifetimeScope();
            builder.Register(c => new QueryFactory(c.Resolve<IKiggDbFactory>(), true)).As<IQueryFactory>().SingleInstance();
        }
    }
}