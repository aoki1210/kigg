using System.Linq;
using System.Data;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class MarkAsSpamRepositoryFixture : BaseIntegrationFixture
    {
        private readonly MarkAsSpamRepository _markAsSpamRepository;

        public MarkAsSpamRepositoryFixture()
        {
            _markAsSpamRepository = new MarkAsSpamRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new MarkAsSpamRepository(_database));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var storyId = GetAnyExistingStory().Id;
            var count = _database.MarkAsSpamDataSource.Count(m => m.StoryId == storyId);
            Assert.Equal(count, _markAsSpamRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindById_Should_Return_Correct_MarkAsSpam()
        {
            var newMarkSpam = CreateNewMarkAsSpam();
            var story = (Story)newMarkSpam.ForStory;
            var user = newMarkSpam.ByUser;
            using(new TransactionScope())
            {
                story.StoryMarkAsSpamsInternal.Add(newMarkSpam);
                _database.SubmitChanges();

                var markSpam = _markAsSpamRepository.FindById(story.Id, user.Id);
                Assert.NotNull(markSpam);
                Assert.Equal(newMarkSpam.ForStory.Id, markSpam.ForStory.Id);
                Assert.Equal(newMarkSpam.ByUser.Id, markSpam.ByUser.Id);
                Assert.Equal(newMarkSpam.FromIPAddress, markSpam.FromIPAddress);
                Assert.Equal(newMarkSpam.MarkedAt, markSpam.MarkedAt);
            }
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var markedSpam = CreateNewMarkAsSpam();
                _markAsSpamRepository.Add(markedSpam);
                Assert.Equal(EntityState.Added, markedSpam.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, markedSpam.EntityState);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var markedSpam = CreateNewMarkAsSpam();
                _markAsSpamRepository.Add(markedSpam);
                _database.SubmitChanges();

                _markAsSpamRepository.Remove(markedSpam);
                Assert.Equal(EntityState.Deleted, markedSpam.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, markedSpam.EntityState);
            }
        }

        private StoryMarkAsSpam CreateNewMarkAsSpam()
        {
            var story = GetAnyExistingStory();
            var user = GetAnyExistingUser();
            return (StoryMarkAsSpam)_domainFactory.CreateMarkAsSpam(story, 
                                                                    SystemTime.Now(), 
                                                                    user, 
                                                                    "127.0.0.1");
        }
    }
}
