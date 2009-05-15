using System;
using System.Linq;
using System.Data;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class StoryViewRepositoryFixture : BaseIntegrationFixture
    {
        private readonly StoryViewRepository _storyViewRepository;

        public StoryViewRepositoryFixture()
        {
            _storyViewRepository = new StoryViewRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new StoryViewRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var view = CreateNewStoryView();
                _storyViewRepository.Add(view);
                Assert.Equal(EntityState.Added, view.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, view.EntityState);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            var view = _database.StoryViewDataSource.First();
            using (new TransactionScope())
            {
                _storyViewRepository.Remove(view);
                Assert.Equal(EntityState.Deleted, view.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, view.EntityState);
            }
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var storyId = GetAnyExistingStory().Id;
            var count = _database.StoryViewDataSource.Count(v => v.Story.Id == storyId);

            Assert.Equal(count, _storyViewRepository.CountByStory(storyId));
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Views()
        {
            var date = new DateTime(2009, 1, 1);
            var storyId = GetAnyExistingStory().Id;

            var count = _database.StoryViewDataSource
                                 .Count(v => v.Story.Id == storyId && v.Timestamp >= date);

            var result = _storyViewRepository.FindAfter(storyId, date);

            Assert.Equal(count, result.Count);
        }

        private StoryView CreateNewStoryView()
        {
            var story = GetAnyExistingStory();
            return (StoryView)_domainFactory.CreateStoryView(story, SystemTime.Now(), "192.168.0.1");
        }
    }
}
