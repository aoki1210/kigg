using System;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Domain.Entities;
    using Infrastructure;
    using Repository;
    using Service;

    using Kigg.Test.Infrastructure;

    public class SpamPostprocessorFixture : BaseFixture
    {
        private readonly Mock<IEventAggregator> _eventAggregator;
        private readonly Mock<IStoryRepository> _storyRepository;

        private readonly SpamPostprocessor _spamPostprocessor;

        public SpamPostprocessorFixture()
        {
            _eventAggregator = new Mock<IEventAggregator>();
            _storyRepository = new Mock<IStoryRepository>();

            _spamPostprocessor = new SpamPostprocessor(unitOfWork.Object, _eventAggregator.Object, _storyRepository.Object);
        }

        [Fact]
        public void Process_For_Story_Should_Log_Warning_When_Story_Is_Spam()
        {
            Process_For_Story_When_Spam();

            log.Verify();
        }

        [Fact]
        public void Process_For_Story_Should_Publish_Event_When_Story_Is_Spam()
        {
            Process_For_Story_When_Spam();

            _eventAggregator.Verify();
        }

        [Fact]
        public void Process_For_Story_Should_Approve_Story_When_Story_Is_Not_Spam()
        {
            var story = new Mock<Story>();

            Process_For_Story_When_Not_Spam(story);

            story.Verify();
        }

        [Fact]
        public void Process_For_Story_Should_Publish_Event_When_Story_Is_Not_Spam()
        {
            var story = new Mock<Story>();

            Process_For_Story_When_Not_Spam(story);

            _eventAggregator.Verify();
        }

        [Fact]
        public void Process_For_Story_Should_Use_StoryRepository()
        {
            Process_For_Story_When_Not_Spam(new Mock<Story>());

            _storyRepository.Verify();
        }

        [Fact]
        public void Process_For_Comment_Should_Log_Warning_When_Comment_Is_Spam()
        {
            Process_For_Comment_When_Spam();

            log.Verify();
        }

        [Fact]
        public void Process_For_Comment_Should_Publish_Event_When_Comment_Is_Spam()
        {
            Process_For_Comment_When_Spam();

            _eventAggregator.Verify();
        }

        [Fact]
        public void Process_For_Comment_Should_Publish_Event_When_Comment_Is_Not_Spam()
        {
            var comment = new Mock<Comment>();
            var story = new Mock<Story>();

            comment.SetupGet(c => c.ForStory).Returns(story.Object);

            _storyRepository.Setup(r => r.FindById(It.IsAny<long>())).Returns(story.Object);
            _eventAggregator.Setup(ea => ea.GetEvent<CommentSubmitEvent>()).Returns(new CommentSubmitEvent()).Verifiable();

            _spamPostprocessor.Process("foo", false, "http://test.com", comment.Object);

            _eventAggregator.Verify();
        }

        [Fact]
        public void Process_For_Comment_Should_Use_StoryRepository()
        {
            Process_For_Comment_When_Spam();

            _storyRepository.Verify();
        }

        private void Process_For_Story_When_Spam()
        {
            _storyRepository.Setup(r => r.FindById(It.IsAny<long>())).Returns(new Mock<Story>().Object).Verifiable();

            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            _eventAggregator.Setup(ea => ea.GetEvent<PossibleSpamStoryEvent>()).Returns(new PossibleSpamStoryEvent()).Verifiable();

            _spamPostprocessor.Process("foo", true, "http://test.com", new Mock<Story>().Object);
        }

        private void Process_For_Story_When_Not_Spam(Mock<Story> story)
        {
            story.SetupGet(s => s.PostedBy).Returns(new Mock<User>().Object);

            _storyRepository.Setup(r => r.FindById(It.IsAny<long>())).Returns(story.Object).Verifiable();
            _eventAggregator.Setup(ea => ea.GetEvent<StorySubmitEvent>()).Returns(new StorySubmitEvent()).Verifiable();

            _spamPostprocessor.Process("foo", false, "http://test.com", new Mock<Story>().Object);
        }

        private void Process_For_Comment_When_Spam()
        {
            var story = new Mock<Story>();

            var comment = new Mock<Comment>();

            comment.SetupGet(c => c.ForStory).Returns(new Mock<Story>().Object);
            comment.SetupGet(c => c.ByUser).Returns(new Mock<User>().Object);
            comment.SetupGet(c => c.ForStory).Returns(story.Object);

            _storyRepository.Setup(r => r.FindById(It.IsAny<long>())).Returns(story.Object);

            story.Setup(s => s.FindComment(It.IsAny<long>())).Returns(comment.Object);

            log.Setup(l => l.Warning(It.IsAny<string>())).Verifiable();
            _eventAggregator.Setup(ea => ea.GetEvent<PossibleSpamCommentEvent>()).Returns(new PossibleSpamCommentEvent()).Verifiable();

            _spamPostprocessor.Process("foo", true, "http://test.com", comment.Object);
        }
    }
}