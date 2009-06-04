using System;
using System.Linq;
using System.Data;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class VoteRepositoryFixture : BaseIntegrationFixture
    {
        private readonly VoteRepository _voteRepository;

        public VoteRepositoryFixture()
        {
            _voteRepository = new VoteRepository(_database);
        }
        
        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new VoteRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var vote = CreateNewStoryVote();
                _voteRepository.Add(vote);
                Assert.Equal(EntityState.Added, vote.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, vote.EntityState);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            var vote = _database.VoteDataSource.First();
            using (new TransactionScope())
            {
                _voteRepository.Remove(vote);
                Assert.Equal(EntityState.Deleted, vote.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, vote.EntityState);
            }
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var story = GetAnyExistingStory();
            var count = story.StoryVotesInternal.CreateSourceQuery().Count();
            Assert.Equal(count, _voteRepository.CountByStory(story.Id));
        }

        [Fact]
        public void FindById_Should_Return_Correct_Vote()
        {
            var loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<StoryVote>(v=>v.User);
            loadOptions.LoadWith<StoryVote>(v=>v.Story);
            _database.LoadOptions = loadOptions;

            var vote = _database.VoteDataSource.First();
            var storyId = vote.StoryId;
            var userId = vote.UserId;

            Assert.NotNull(_voteRepository.FindById(storyId, userId));
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Votes()
        {
            var date = new DateTime(2009,1,1);
            var storyId = GetAnyExistingStory().Id;
            var count = _database.VoteDataSource.Count(v => v.StoryId == storyId && v.Timestamp >= date);
            var result = _voteRepository.FindAfter(storyId, date);

            Assert.Equal(count, result.Count);
        }

        private StoryVote CreateNewStoryVote()
        {
            var story = CreateNewStory();
            var user = CreateNewUser();
            return (StoryVote)_domainFactory.CreateStoryVote(story, SystemTime.Now(), user,"127.0.0.1");
        }
    }
}
