namespace Kigg.Web
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Web;

    using Autofac;

    using Configuration;
    using Caching;
    using Security;
    using Domain.Entities;
    using Infrastructure;
    using Infrastructure.EntityFramework;
    using Infrastructure.EntityFramework.Query;
    using UnitOfWork = Infrastructure.EntityFramework.UnitOfWork;
    using Services;

    public class AutofacServiceRegistration : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.Register(c => new CacheManager(HttpRuntime.Cache)).As<ICacheManager>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<OpenIdRelyingParty>().As<IOpenIdRelyingParty>().InstancePerDependency();
            builder.RegisterType<FormsAuthentication>().As<IFormsAuthentication>().SingleInstance();
            builder.RegisterType<Cookie>().As<ICookie>().SingleInstance();

            Settings settings = CreateSettings();
            builder.RegisterInstance(settings).As<Settings>().SingleInstance();

            RegisterRepositories(builder);
            RegisterServices(builder);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerLifetimeScope();
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

        private static Settings CreateSettings()
        {
            var section = (KiggConfigurationSection)ConfigurationManager.GetSection(KiggConfigurationSection.SectionName);

            var thumbnailSettings = new ThumbnailSettings(section.Thumbnail.ApiKey, section.Thumbnail.Endpoint);
            /*
            IList<WebAsset> assets = new List<WebAsset>();
            foreach (AssetConfigurationElement configuration in section.Assets)
            {
                assets.Add(new WebAsset
                               {
                                   Url = configuration.Url,
                                   Group = configuration.Group,
                                   Type = configuration.Type
                               });
            }

            var assetSettings = new AssetSettings(section.Assets.Theme,
                                                  section.Assets.ScriptFilesPath,
                                                  section.Assets.StyleSheetFilesPath,
                                                  section.Assets.Compress,
                                                  assets);
            */
            TwitterSettings twitterSettings = null;

            if (section.Twitter != null)
            {
                twitterSettings = new TwitterSettings(section.Twitter.UserName, section.Twitter.Password, section.Twitter.Endpoint, section.Twitter.MessageTemplate, section.Twitter.MaximumMessageLength, section.Twitter.Recipients.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            }

            IList<User> defaultUsers = new List<User>();

            foreach (DefaultUserConfigurationElement configuration in section.DefaultUsers)
            {
                string hashedPassword = null;

                if (!string.IsNullOrWhiteSpace(configuration.Password))
                {
                    hashedPassword = configuration.Password.Trim().Hash();
                }

                defaultUsers.Add(new User
                                     {
                                         UserName = configuration.UserName,
                                         Password = hashedPassword,
                                         About = configuration.Description,
                                         Email = configuration.Email,
                                         Role = configuration.Role,
                                         IsActive = true,
                                         CreatedAt = SystemTime.Now,
                                         LastActivityAt = SystemTime.Now,
                                     });
            }


            IList<Category> defaultCategories = new List<Category>();

            foreach (DefaultCategoryConfigurationElement configuration in section.DefaultCategories)
            {
                defaultCategories.Add(new Category
                                          {
                                              Name = configuration.Name,
                                              UniqueName = configuration.UniqueName,
                                              CreatedAt = SystemTime.Now
                                          });
            }
            var settings = new Settings(thumbnailSettings, /*assetSettings,*/ twitterSettings, defaultUsers, defaultCategories);

            return settings;
        }
    }
}