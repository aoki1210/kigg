using System.Linq;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;

    public class CommentSubscribtionRepositoryFixture: BaseIntegrationFixture
    {
        private readonly CommentSubscribtionRepository _commentSubscribtionRepository;

        public CommentSubscribtionRepositoryFixture()
        {
            _commentSubscribtionRepository = new CommentSubscribtionRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CommentSubscribtionRepository(_dbFactory));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var story = GetAnyExistingStory();
            using(new TransactionScope())
            {
                story.SubscribeComment(GetAnyExistingUser());
                _database.SubmitChanges();

                var count = story.CommentSubscribersInternal.CreateSourceQuery().Count();
                
                Assert.True(count > 0);
                
                Assert.Equal(count, _commentSubscribtionRepository.CountByStory(story.Id));
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_Subscribtion()
        {
            var story = GetAnyExistingStory();
            var user = GetAnyExistingUser();

            var storyId = story.Id;
            var userId = user.Id;

            using (new TransactionScope())
            {
                story.SubscribeComment(user);
                _database.SubmitChanges();

                var count = story.CommentSubscribersInternal.CreateSourceQuery().Count();

                Assert.True(count > 0);

                var commentSubscribtion = _commentSubscribtionRepository.FindById(storyId, userId);
                Assert.NotNull(commentSubscribtion);
                Assert.Equal(commentSubscribtion.ForStory.Id, storyId);
                Assert.Equal(commentSubscribtion.ByUser.Id, userId);
            }
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            var story = GetAnyExistingStory();
            var user = GetAnyExistingUser();
            var count = story.CommentSubscribersInternal.CreateSourceQuery().Count();
            using (new TransactionScope())
            {
                var subscribtion = _domainFactory.CreateCommentSubscribtion(story, user);
                _commentSubscribtionRepository.Add(subscribtion);
                _database.SubmitChanges();
                var newCount = story.CommentSubscribersInternal.CreateSourceQuery().Count();
                Assert.Equal(count + 1, newCount);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            var story = GetAnyExistingStory();
            var user = GetAnyExistingUser();
            var count = story.CommentSubscribersInternal.CreateSourceQuery().Count();
            using (new TransactionScope())
            {
                var subscribtion = _domainFactory.CreateCommentSubscribtion(story, user);
                
                _commentSubscribtionRepository.Add(subscribtion);
                _database.SubmitChanges();

                _commentSubscribtionRepository.Remove(subscribtion);
                _database.SubmitChanges();

                var newCount = story.CommentSubscribersInternal.CreateSourceQuery().Count();
                Assert.Equal(count, newCount);
            }
        }
    }
}
