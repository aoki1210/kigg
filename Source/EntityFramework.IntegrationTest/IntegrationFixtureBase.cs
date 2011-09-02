namespace Kigg.Infrastructure.EntityFramework.IntegrationTest
{
    using System.Data.Common;
    using System.Configuration;

    using DomainObjects;

    public abstract class IntegrationFixtureBase : Disposable
    {
        protected readonly KiggDbFactory dbFactory;
        protected KiggDbContext Context
        {
            get { return dbFactory.Get(); }
        }

        protected IntegrationFixtureBase()
        {
            var connConfig = ConfigurationManager.ConnectionStrings["KiGG"];
            DbProviderFactory provider = DbProviderFactories.GetFactory(connConfig.ProviderName);
            var connString = connConfig.ConnectionString;
            dbFactory = new KiggDbFactory(provider, connString);
        }

        protected Category NewCategory(bool persist)
        {
            var category = new Category { Name = "C#", UniqueName = "C-Sharp", CreatedAt = SystemTime.Now() };

            if(persist)
            {
                Context.Categories.Add(category);

                Context.SaveChanges();
            }

            return category;
        }

        protected override void DisposeCore()
        {
            if(dbFactory != null)
            {
                dbFactory.Dispose();
            }
        }
    }
}
