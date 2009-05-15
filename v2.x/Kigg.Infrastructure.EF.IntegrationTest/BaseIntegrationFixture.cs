using System.Linq;
using Kigg.EF.DomainObjects;
using Kigg.EF.Repository;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    public abstract class BaseIntegrationFixture
    {
        protected readonly Database _database;
        protected readonly DatabaseFactory _dbFactory;
        protected readonly DomainObjectFactory _domainFactory;
        private readonly ConnectionString _connectionString;
        
        protected BaseIntegrationFixture()
        {
            _connectionString = new ConnectionString(new ConfigurationManagerWrapper(), "KiGG");
            _database = new Database(_connectionString.Value);
            _dbFactory = new DatabaseFactory(_connectionString);
            _domainFactory = new DomainObjectFactory();
        }
        
        protected string ConnectionString
        {
            get
            {
                return _connectionString.Value; 
            }
        }

        protected Story CreateNewStory(Category forCategory, User byUser, string fromIpAddress, string title, string description, string url)
        {
            return (Story)_domainFactory.CreateStory(forCategory, byUser,
                                               fromIpAddress, title,
                                               description, url);
        }

        protected Story CreateNewStory()
        {
            return CreateNewStory(CreateNewCategory(), CreateNewUser(), 
                                "127.0.0.1", "dummy story",
                                "dummy description", "http://dummy.com/story.aspx");
        }

        protected Category CreateNewCategory()
        {
            return (Category)_domainFactory.CreateCategory("Dummy Category");
        }

        protected User CreateNewUser()
        {
            return (User)_domainFactory.CreateUser("dummy", "dummy@mail.com", "Pa$$w0rd");
        }

        protected Tag CreateNewTag()
        {
            return (Tag)_domainFactory.CreateTag("DummyTag");
        }

        protected Story GetAnyExistingStory()
        {
            return _database.StoryDataSource.First();
        }

        protected User GetAnyExistingUser()
        {
            return _database.UserDataSource.First();
        }

        protected StoryComment CreateNewComment()
        {
            var story = GetAnyExistingStory();
            var user = GetAnyExistingUser();
            return (StoryComment)_domainFactory.CreateComment(story, "dummy comment", SystemTime.Now(), user, "127.0.0.1");
        }
    }
}
