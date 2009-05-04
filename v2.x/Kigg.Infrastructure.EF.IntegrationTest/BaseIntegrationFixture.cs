using System;
using Kigg.EF.DomainObjects;
using Kigg.EF.Repository;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    public abstract class BaseIntegrationFixture
    {
        protected const string _assemblyInfo = "Kigg.Infrastructure.EF, Version=2.2.0.0, Culture=neutral, PublicKeyToken=88117f6fba1a09d8";
        protected const string _edmFilesPrefix = "Kigg.Infrastructure.EF.EDM.DomainObjects";
        protected const string _edmConnStringFormat = "metadata=res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl;provider=System.Data.SqlClient;provider connection string=\"{2}\"";
        protected const string _providerConnString = "Data Source=.\\sqlexpress;Initial Catalog=KiGG;Integrated Security=True;MultipleActiveResultSets=True";
        
        protected readonly Database _database;
        protected readonly DomainObjectFactory _factory;

        protected BaseIntegrationFixture()
        {
            _database = new Database(ConnectionString);
            _factory = new DomainObjectFactory();
        }
        protected string ConnectionString
        {
            get
            {
                return String.Format(_edmConnStringFormat, _assemblyInfo, _edmFilesPrefix, _providerConnString);
            }
        }

        protected Story CreateStory(Category forCategory, User byUser, string fromIpAddress, string title, string description, string url)
        {
            return (Story)_factory.CreateStory(forCategory, byUser,
                                               fromIpAddress, title,
                                               description, url);
        }

        protected Story CreateStory()
        {
            return CreateStory(CreateCategory(), CreateUser(), 
                                "127.0.0.1", "dummy story",
                                "dummy description", "http://dummy.com/story.aspx");
        }

        protected Category CreateCategory()
        {
            return (Category)_factory.CreateCategory("Dummy Category");
        }
        protected User CreateUser()
        {
            return (User)_factory.CreateUser("dummy", "dummy@mail.com", "Pa$$w0rd");
        }

        protected Tag CraeteTag()
        {
            return (Tag)_factory.CreateTag("DummyTag");
        }
    }
}
