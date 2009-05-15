using System;

using Moq;
using Xunit;

namespace Kigg.Infrastructure.EF.Test
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class DatabaseFactoryFixture
    {
        private readonly string _connectionString;
        
        private const string _edmFilesPrefix = "Kigg.Infrastructure.EF.EDM.DomainObjects";
        private const string _edmConnStringFormat = "metadata=res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl;provider=System.Data.SqlClient;";

        private readonly IDatabaseFactory _factory;

        public DatabaseFactoryFixture()
        {
            _connectionString = String.Format(_edmConnStringFormat, 
                                              typeof(Story).Assembly.FullName, 
                                              _edmFilesPrefix);

            var connectionString = new Mock<IConnectionString>();
            
            connectionString.SetupGet(c => c.Value).Returns(_connectionString);
            _factory = new DatabaseFactory(connectionString.Object);
        }

        [Fact]
        public void Get_Should_Return_The_Same_Database()
        {
            var db1 = _factory.Get();
            var db2 = _factory.Get();

            Assert.Same(db1, db2);
        }

        [Fact]
        public void Accessing_Database_After_Dispose_Should_Throw_Exception()
        {
            var db = _factory.Get();
            _factory.Dispose();

            Assert.Throws<ObjectDisposedException>(db.SubmitChanges);
        }
    }
}