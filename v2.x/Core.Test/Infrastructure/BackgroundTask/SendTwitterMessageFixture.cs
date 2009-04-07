using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Infrastructure;
    using Service;

    using Kigg.Test.Infrastructure;

    public class SendTwitterMessageFixture : BaseFixture
    {
        private readonly Mock<IEventAggregator> _eventAggregator;
        private readonly Mock<IContentService> _contentService;
        private readonly Mock<IHttpForm> _httpForm;

        private readonly SendTwitterMessage _sendTwitterMessage;

        public SendTwitterMessageFixture()
        {
            _eventAggregator = new Mock<IEventAggregator>();
            _contentService = new Mock<IContentService>();
            _httpForm = new Mock<IHttpForm>();

            _sendTwitterMessage = new SendTwitterMessage(_eventAggregator.Object, _contentService.Object, _httpForm.Object, "dummy", "dummy", "dummy", "http:/twitter.com/status", false, "http:/twitter.com/message", "xxx|yyy|zzz");
        }

        [Fact]
        public void StorySubmitted_Should_Update_Status()
        {
            var story = MockStory();

            _contentService.Expect(cs => cs.ShortUrl(It.IsAny<string>())).Returns("http://shorturl.com/axs").Verifiable();
            _httpForm.Expect(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            _sendTwitterMessage.StorySubmitted(new StorySubmitEventArgs(story.Object, "http://dotnetshoutout.com/dummy-story"));

            _contentService.Verify();
            _httpForm.Verify();
        }

        [Fact]
        public void StoryApproved_Should_Update_Status()
        {
            var story = MockStory();

            _contentService.Expect(cs => cs.ShortUrl(It.IsAny<string>())).Returns("http://shorturl.com/axs").Verifiable();
            _httpForm.Expect(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            _sendTwitterMessage.StoryApproved(new StoryApproveEventArgs(story.Object, new Mock<IUser>().Object,"http://dotnetshoutout.com/dummy-story"));

            _contentService.Verify();
            _httpForm.Verify();
        }

        [Fact]
        public void StoryPublished_Should_Update_Status()
        {
            var story = MockStory();

            var stories = new List<PublishedStory>
                              {
                                  new PublishedStory(story.Object)
                              };

            _contentService.Expect(cs => cs.ShortUrl(It.IsAny<string>())).Returns("http://shorturl.com/axs").Verifiable();
            _httpForm.Expect(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            _sendTwitterMessage.StoryPublished(new StoryPublishEventArgs(stories, SystemTime.Now()));

            _contentService.Verify();
            _httpForm.Verify();
        }

        [Fact]
        public void PossibleSpamStoryDetected_Should_Send_Message()
        {
            var story = new Mock<IStory>();
            story.ExpectGet(s => s.Url).Returns("http://astory.com");
            story.ExpectGet(s => s.Title).Returns("dummy story");

            _contentService.Expect(cs => cs.ShortUrl(It.IsAny<string>())).Returns("http://shorturl.com/axs").Verifiable();
            _httpForm.Expect(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            _sendTwitterMessage.PossibleSpamStoryDetected(new PossibleSpamStoryEventArgs(story.Object, "default", "http://dotnetshoutout.com/dummy-story"));

            _contentService.Verify();
            _httpForm.Verify();
        }

        [Fact]
        public void PossibleSpamCommentDetected_Should_Send_Message()
        {
            var story = new Mock<IStory>();
            story.ExpectGet(s => s.Url).Returns("http://astory.com");
            story.ExpectGet(s => s.Title).Returns("dummy story");

            var comment = new Mock<IComment>();
            comment.ExpectGet(c => c.ForStory).Returns(story.Object);

            _contentService.Expect(cs => cs.ShortUrl(It.IsAny<string>())).Returns("http://shorturl.com/axs").Verifiable();
            _httpForm.Expect(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            _sendTwitterMessage.PossibleSpamCommentDetected(new PossibleSpamCommentEventArgs(comment.Object, "default", "http://dotnetshoutout.com/dummy-story"));

            _contentService.Verify();
            _httpForm.Verify();
        }

        [Fact]
        public void StoryMarkedAsSpam_Should_Send_Message()
        {
            var story = new Mock<IStory>();
            story.ExpectGet(s => s.Url).Returns("http://astory.com");
            story.ExpectGet(s => s.Title).Returns("dummy story");

            _contentService.Expect(cs => cs.ShortUrl(It.IsAny<string>())).Returns("http://shorturl.com/axs").Verifiable();
            _httpForm.Expect(h => h.PostAsync(It.IsAny<HttpFormPostRequest>())).Verifiable();

            _sendTwitterMessage.StoryMarkedAsSpam(new StoryMarkAsSpamEventArgs(story.Object, new Mock<IUser>().Object, "http://dotnetshoutout.com/dummy-story"));

            _contentService.Verify();
            _httpForm.Verify();
        }

        [Fact]
        public void Start_Should_Subscribe_Events()
        {
            var storySubmitEvent = new Mock<StorySubmitEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StorySubmitEvent>()).Returns(storySubmitEvent.Object).Verifiable();
            storySubmitEvent.Expect(e => e.Subscribe(It.IsAny<Action<StorySubmitEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Expect(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken());

            var storyPublishEvent = new Mock<StoryPublishEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StoryPublishEvent>()).Returns(storyPublishEvent.Object).Verifiable();
            storyPublishEvent.Expect(e => e.Subscribe(It.IsAny<Action<StoryPublishEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var possibleSpamStoryEvent = new Mock<PossibleSpamStoryEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<PossibleSpamStoryEvent>()).Returns(possibleSpamStoryEvent.Object).Verifiable();
            possibleSpamStoryEvent.Expect(e => e.Subscribe(It.IsAny<Action<PossibleSpamStoryEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var possibleSpamCommentEvent = new Mock<PossibleSpamCommentEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<PossibleSpamCommentEvent>()).Returns(possibleSpamCommentEvent.Object).Verifiable();
            possibleSpamCommentEvent.Expect(e => e.Subscribe(It.IsAny<Action<PossibleSpamCommentEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            var storyMarkAsSpamEvent = new Mock<StoryMarkAsSpamEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StoryMarkAsSpamEvent>()).Returns(storyMarkAsSpamEvent.Object).Verifiable();
            storyMarkAsSpamEvent.Expect(e => e.Subscribe(It.IsAny<Action<StoryMarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken()).Verifiable();

            _sendTwitterMessage.Start();

            _eventAggregator.Verify();

            storySubmitEvent.Verify();
            storyApproveEvent.Verify();
            storyPublishEvent.Verify();
            possibleSpamStoryEvent.Verify();
            possibleSpamCommentEvent.Verify();
            storyMarkAsSpamEvent.Verify();
        }

        [Fact]
        public void Stop_Should_Unsubscribe_Events()
        {
            var storySubmitEvent = new Mock<StorySubmitEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StorySubmitEvent>()).Returns(storySubmitEvent.Object).Verifiable();
            storySubmitEvent.Expect(e => e.Subscribe(It.IsAny<Action<StorySubmitEventArgs>>(), true)).Returns(new SubscriptionToken());
            storySubmitEvent.Expect(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyApproveEvent = new Mock<StoryApproveEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StoryApproveEvent>()).Returns(storyApproveEvent.Object).Verifiable();
            storyApproveEvent.Expect(e => e.Subscribe(It.IsAny<Action<StoryApproveEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyApproveEvent.Expect(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyPublishEvent = new Mock<StoryPublishEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StoryPublishEvent>()).Returns(storyPublishEvent.Object).Verifiable();
            storyPublishEvent.Expect(e => e.Subscribe(It.IsAny<Action<StoryPublishEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyPublishEvent.Expect(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var possibleSpamStoryEvent = new Mock<PossibleSpamStoryEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<PossibleSpamStoryEvent>()).Returns(possibleSpamStoryEvent.Object).Verifiable();
            possibleSpamStoryEvent.Expect(e => e.Subscribe(It.IsAny<Action<PossibleSpamStoryEventArgs>>(), true)).Returns(new SubscriptionToken());
            possibleSpamStoryEvent.Expect(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var possibleSpamCommentEvent = new Mock<PossibleSpamCommentEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<PossibleSpamCommentEvent>()).Returns(possibleSpamCommentEvent.Object).Verifiable();
            possibleSpamCommentEvent.Expect(e => e.Subscribe(It.IsAny<Action<PossibleSpamCommentEventArgs>>(), true)).Returns(new SubscriptionToken());
            possibleSpamCommentEvent.Expect(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            var storyMarkAsSpamEvent = new Mock<StoryMarkAsSpamEvent>();

            _eventAggregator.Expect(ea => ea.GetEvent<StoryMarkAsSpamEvent>()).Returns(storyMarkAsSpamEvent.Object).Verifiable();
            storyMarkAsSpamEvent.Expect(e => e.Subscribe(It.IsAny<Action<StoryMarkAsSpamEventArgs>>(), true)).Returns(new SubscriptionToken());
            storyMarkAsSpamEvent.Expect(e => e.Unsubscribe(It.IsAny<SubscriptionToken>())).Verifiable();

            _sendTwitterMessage.Start();
            _sendTwitterMessage.Stop();

            _eventAggregator.Verify();

            storySubmitEvent.Verify();
            storyApproveEvent.Verify();
            storyPublishEvent.Verify();
            possibleSpamStoryEvent.Verify();
            possibleSpamCommentEvent.Verify();
            storyMarkAsSpamEvent.Verify();
        }

        private static Mock<IStory> MockStory()
        {
            var tag1 = new Mock<ITag>();
            tag1.ExpectGet(t => t.Name).Returns("Tag1");

            var tag2 = new Mock<ITag>();
            tag2.ExpectGet(t => t.Name).Returns("Tag2");

            var story = new Mock<IStory>();
            story.ExpectGet(s => s.Url).Returns("http://astory.com");
            story.ExpectGet(s => s.Title).Returns(new string('x', 105));
            story.ExpectGet(s => s.Tags).Returns(new List<ITag> { tag1.Object, tag2.Object });

            return story;
        }
    }
}