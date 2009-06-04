using System.Linq;
using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;

    public class ConnectionStringFixture
    {
        private readonly ConfigurationManagerWrapper _configManager;
        public ConnectionStringFixture()
        {
            _configManager = new ConfigurationManagerWrapper();
        }

        [Fact]
        public void Initialize_ConnectionString_With_ConfigurationManager_Should_Succeed()
        {
            #pragma warning disable 168
            var connectionString = new ConnectionString(_configManager, "KiGG");
            #pragma warning restore 168
        }

        [Fact]
        public void Connecting_To_Database_Using_ConnectionString_Should_Succeed()
        {
            //Connection string must be provided correctlly in app.config
            var connectionString = new ConnectionString(_configManager, "KiGG");
            using(var database = new Database(connectionString.Value))
            {
                #pragma warning disable 168
                var user = database.UserDataSource.First();
                #pragma warning restore 168
            }
        }
    }
}
