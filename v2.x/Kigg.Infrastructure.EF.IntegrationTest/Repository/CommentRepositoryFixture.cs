using System;
using System.Linq;
using System.Data;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;

    public class CommentRepositoryFixture : BaseIntegrationFixture
    {
        private readonly CommentRepository _commentRepository;

        public CommentRepositoryFixture()
        {
            _commentRepository = new CommentRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CommentRepository(_dbFactory));
        }

        [Fact]
        public void CountByStory_Should_Return_Correct_Count()
        {
            var story = GetStoryWithComments();
            var count = story.StoryCommentsInternal.CreateSourceQuery().Count();
            Assert.Equal(count, _commentRepository.CountByStory(story.Id));
        }

        [Fact]
        public void FindById_Should_Return_Correct_Comment()
        {
            var story = GetStoryWithComments();
            var comment = _database.CommentDataSource.Where(c => c.Story.Id == story.Id).First();
            var foundComment = _commentRepository.FindById(story.Id, comment.Id);
            Assert.NotNull(foundComment);
            Assert.Equal(comment.Id, foundComment.Id);
        }

        [Fact]
        public void FindAfter_Should_Return_Correct_Comments()
        {
            var date = new DateTime(2000, 1, 1);
            var story = GetStoryWithComments();

            var count = _database.CommentDataSource
                                 .Where(c => c.Story.Id == story.Id && c.CreatedAt >= date)
                                 .Count();
            
            var result = _commentRepository.FindAfter(story.Id, date);

            Assert.Equal(count, result.Count);
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var comment = CreateNewComment();
                _commentRepository.Add(comment);
                Assert.Equal(EntityState.Added, comment.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, comment.EntityState);
            }
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            var comment = _database.CommentDataSource.First();
            using (new TransactionScope())
            {
                _commentRepository.Remove(comment);
                Assert.Equal(EntityState.Deleted, comment.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, comment.EntityState);
            }
        }

        private Story GetStoryWithComments()
        {
            return _database.StoryDataSource.Where(s => s.StoryCommentsInternal.Any(c => c.Story.Id == s.Id)).First();
        }
    }
}
