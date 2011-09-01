namespace Kigg.Infrastructure.EntityFramework.IntegrationTest
{
    using System.Data.Common;
    using System.Configuration;

    public abstract class BaseIntegrationFixture : Disposable
    {
        protected readonly KiggDbFactory dbFactory;
        
        protected BaseIntegrationFixture()
        {
            var connConfig = ConfigurationManager.ConnectionStrings["KiGG"];
            DbProviderFactory provider = DbProviderFactories.GetFactory(connConfig.ProviderName);
            var connString = connConfig.ConnectionString;
            dbFactory = new KiggDbFactory(provider, connString);
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
