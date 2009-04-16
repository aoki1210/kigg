using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;
    using Infrastructure;
    using Infrastructure.DomainRepositoryExtensions;
    using Kigg.Test.Infrastructure;

    public class StoryExtensionFixture : BaseFixture
    {
        private readonly Mock<IStory> _story;

        public StoryExtensionFixture()
        {
            _story = new Mock<IStory>();
        }

        [Fact]
        public void IsNew_Should_Be_True_When_LastProcessedAt_Is_Null()
        {
            _story.SetupGet(s => s.LastProcessedAt).Returns((DateTime?)null);

            Assert.True(_story.Object.IsNew());
        }

        [Fact]
        public void IsPublished_Should_Be_True_When_PublishedAt_Is_Not_Null()
        {
            _story.SetupGet(s => s.PublishedAt).Returns(SystemTime.Now());

            Assert.True(_story.Object.IsPublished());
        }

        [Fact]
        public void HasExpired_Should_Be_True_When_CreatedAt_Is_Greater_Than_MaximumAgeToPublish()
        {
            var date = SystemTime.Now().AddHours(-(settings.Object.MaximumAgeOfStoryInHoursToPublish + 1));

            _story.SetupGet(s => s.CreatedAt).Returns(date);

            Assert.True(_story.Object.HasExpired());
        }

        [Fact]
        public void IsApproved_Should_Be_True_When_Aapproved_Is_Not_Null()
        {
            _story.SetupGet(s => s.ApprovedAt).Returns(SystemTime.Now());

            Assert.True(_story.Object.IsApproved());
        }

        [Fact]
        public void HasComments_Should_Be_True_When_CommentCount_Is_Greater_Than_Zero()
        {
            _story.SetupGet(s => s.CommentCount).Returns(1);

            Assert.True(_story.Object.HasComments());
        }

        [Fact]
        public void IsPostedBy_Should_Be_True_When_PostedBy_User_Id_Is_Same()
        {
            var userId = Guid.NewGuid();

            var postedByUser = new Mock<IUser>();

            postedByUser.SetupGet(u => u.Id).Returns(userId);
            _story.SetupGet(s => s.PostedBy).Returns(postedByUser.Object);

            var checkingUser = new Mock<IUser>();
            checkingUser.SetupGet(u => u.Id).Returns(userId);

            Assert.True(_story.Object.IsPostedBy(checkingUser.Object));
        }

        [Fact]
        public void Host_Returns_The_Domain_Name_From_The_Story_Url()
        {
            _story.SetupGet(s => s.Url).Returns("http://weblogs.asp.net/rashid");

            Assert.Equal("weblogs.asp.net", _story.Object.Host());
        }

        [Fact]
        public void SmallThumbnail_Uses_Thumbnail_To_Build_Url()
        {
            thumbnail.Setup(t => t.For(It.IsAny<string>(), ThumbnailSize.Small)).Returns("http://thumbnail.com").Verifiable();

            _story.SetupGet(s => s.Url).Returns("http://dotnetshoutout.com");

            _story.Object.SmallThumbnail();

            thumbnail.Verify();
        }

        [Fact]
        public void MediumThumbnail_Uses_Thumbnail_To_Build_Url()
        {
            thumbnail.Setup(t => t.For(It.IsAny<string>(), ThumbnailSize.Medium)).Returns("http://thumbnail.com").Verifiable();

            _story.SetupGet(s => s.Url).Returns("http://dotnetshoutout.com");

            _story.Object.MediumThumbnail();

            thumbnail.Verify();
        }

        [Fact]
        public void GetViewCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<IStoryViewRepository>(10);
            int i = _story.Object.GetViewCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetVoteCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<IVoteRepository>(10);
            int i = _story.Object.GetVoteCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetMarkAsSpamCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<IMarkAsSpamRepository>(10);
            int i = _story.Object.GetMarkAsSpamCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetCommentCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<ICommentRepository>(10);
            int i = _story.Object.GetCommentCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void GetSubscriberCount_Should_Return_Correct_Value()
        {
            SetupCountByStoryRepository<ICommentSubscribtionRepository>(10);
            int i = _story.Object.GetSubscriberCount();

            Assert.True(i == 10);
        }

        [Fact]
        public void AddView_Should_Use_DomainObjectFactory_And_StoryViewRepository()
        {
            var repository = CreateAndSetupMock<IStoryViewRepository>();
            var domFactory = CreateAndSetupMock<IDomainObjectFactory>();
            
            DateTime now = SystemTime.Now();

            //Create through DomainObjectFactory
            domFactory.Setup(f => f.CreateStoryView(_story.Object, now, "127.0.0.1")).Returns(It.IsAny<IStoryView>()).Verifiable();
            
            //Add through StoryViewRepository
            repository.Setup(r=>r.Add(It.IsAny<IStoryView>())).Verifiable();
            
            _story.Object.AddView(now, "127.0.0.1");

            domFactory.Verify();
            repository.Verify();
            
        }
        
        [Fact]
        public void AddVote_Should_Use_DomainObjectFactory_And_VoteRepository()
        {
            var repository = CreateAndSetupMock<IVoteRepository>();
            var domFactory = CreateAndSetupMock<IDomainObjectFactory>();

            var now = SystemTime.Now();

            //Create through DomainObjectFactory
            domFactory.Setup(f => f.CreateStoryVote(_story.Object, now, It.IsAny<IUser>(), "127.0.0.1")).Returns(It.IsAny<IVote>()).Verifiable();

            //Add through StoryViewRepository
            repository.Setup(r => r.Add(It.IsAny<IVote>())).Verifiable();

            _story.Object.AddVote(now, new Mock<IUser>().Object, "127.0.0.1");

            domFactory.Verify();
            repository.Verify();
        }
        
        [Fact]
        public void RemoveVote_Should_Use_VoteRepository()
        {
            var repository = CreateAndSetupMock<IVoteRepository>();

            repository.Setup(r => r.FindById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(It.IsAny<IVote>());
            repository.Setup(r => r.Remove(It.IsAny<IVote>()));

            _story.Object.RemoveVote(SystemTime.Now(), new Mock<IUser>().Object);

            repository.VerifyAll();
        }

        private void SetupCountByStoryRepository<T>(int count) where T : class, ICountByStoryRepository
        {
            var repository = CreateAndSetupMock<T>();
            repository.Setup(r => r.CountByStory(_story.Object.Id)).Returns(count);
        }

        private Mock<T> CreateAndSetupMock<T>() where T: class
        {
            var mock = new Mock<T>();
            resolver.Setup(r => r.Resolve<T>()).Returns(mock.Object);
            return mock;
        }
    }
}
