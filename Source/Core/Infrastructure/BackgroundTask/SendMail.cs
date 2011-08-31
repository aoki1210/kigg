namespace Kigg.Infrastructure
{
    using System;

    using Service;

    public class SendMail : BaseBackgroundTask
    {
        private readonly IEmailSender emailSender;

        private SubscriptionToken commentSubmitToken;
        private SubscriptionToken commentMarkAsOffendedToken;
        private SubscriptionToken commentSpamToken;
        private SubscriptionToken storyApproveToken;
        private SubscriptionToken storyDeleteToken;
        private SubscriptionToken storyMarkAsSpamToken;
        private SubscriptionToken storySpamToken;
        private SubscriptionToken storyPublishToken;
        private SubscriptionToken possibleStorySpamToken;
        private SubscriptionToken possibleCommentSpamToken;

        public SendMail(IEventAggregator eventAggregator, IEmailSender emailSender) : base(eventAggregator)
        {
            Check.Argument.IsNotNull(emailSender, "emailSender");

            this.emailSender = emailSender;
        }

        protected override void OnStart()
        {
            if (!IsRunning)
            {
                commentSubmitToken = Subscribe<CommentSubmitEvent, CommentSubmitEventArgs>(CommentSubmitted);
                commentMarkAsOffendedToken = Subscribe<CommentMarkAsOffendedEvent, CommentMarkAsOffendedEventArgs>(CommentMarkedAsOffended);
                commentSpamToken = Subscribe<CommentSpamEvent, CommentSpamEventArgs>(CommentSpammed);
                storyApproveToken = Subscribe<StoryApproveEvent, StoryApproveEventArgs>(StoryApproved);
                storyDeleteToken = Subscribe<StoryDeleteEvent, StoryDeleteEventArgs>(StoryDeleted);
                storyMarkAsSpamToken = Subscribe<StoryMarkAsSpamEvent, StoryMarkAsSpamEventArgs>(StoryMarkedAsSpam);
                storySpamToken = Subscribe<StorySpamEvent, StorySpamEventArgs>(StorySpammed);
                storyPublishToken = Subscribe<StoryPublishEvent, StoryPublishEventArgs>(StoryPublished);
                possibleStorySpamToken = Subscribe<PossibleSpamStoryEvent, PossibleSpamStoryEventArgs>(PossibleSpamStoryDetected);
                possibleCommentSpamToken = Subscribe<PossibleSpamCommentEvent, PossibleSpamCommentEventArgs>(PossibleSpamCommentDetected);
            }
        }

        protected override void OnStop()
        {
            if (IsRunning)
            {
                Unsubscribe<CommentSubmitEvent>(commentSubmitToken);
                Unsubscribe<CommentMarkAsOffendedEvent>(commentMarkAsOffendedToken);
                Unsubscribe<CommentSpamEvent>(commentSpamToken);
                Unsubscribe<StoryApproveEvent>(storyApproveToken);
                Unsubscribe<StoryDeleteEvent>(storyDeleteToken);
                Unsubscribe<StoryMarkAsSpamEvent>(storyMarkAsSpamToken);
                Unsubscribe<StorySpamEvent>(storySpamToken);
                Unsubscribe<StoryPublishEvent>(storyPublishToken);
                Unsubscribe<PossibleSpamStoryEvent>(possibleStorySpamToken);
                Unsubscribe<PossibleSpamCommentEvent>(possibleCommentSpamToken);
            }
        }

        internal void CommentSubmitted(CommentSubmitEventArgs eventArgs)
        {
            throw new NotImplementedException();
            //_emailSender.SendComment(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.Comment.ForStory.Subscribers);
        }

        internal void CommentMarkedAsOffended(CommentMarkAsOffendedEventArgs eventArgs)
        {
            emailSender.NotifyCommentAsOffended(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.User);
        }

        internal void CommentSpammed(CommentSpamEventArgs eventArgs)
        {
            emailSender.NotifyConfirmSpamComment(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.User);
        }

        internal void StoryApproved(StoryApproveEventArgs eventArgs)
        {
            emailSender.NotifyStoryApprove(eventArgs.DetailUrl, eventArgs.Story, eventArgs.User);
        }

        internal void StoryDeleted(StoryDeleteEventArgs eventArgs)
        {
            emailSender.NotifyStoryDelete(eventArgs.Story, eventArgs.User);
        }

        internal void StoryMarkedAsSpam(StoryMarkAsSpamEventArgs eventArgs)
        {
            emailSender.NotifyStoryMarkedAsSpam(eventArgs.DetailUrl, eventArgs.Story, eventArgs.User);
        }

        internal void StorySpammed(StorySpamEventArgs eventArgs)
        {
            emailSender.NotifyConfirmSpamStory(eventArgs.DetailUrl, eventArgs.Story, eventArgs.User);
        }

        internal void StoryPublished(StoryPublishEventArgs eventArgs)
        {
            emailSender.NotifyPublishedStories(eventArgs.Timestamp, eventArgs.PublishedStories);
        }

        internal void PossibleSpamStoryDetected(PossibleSpamStoryEventArgs eventArgs)
        {
            emailSender.NotifySpamStory(eventArgs.DetailUrl, eventArgs.Story, eventArgs.Source);
        }

        internal void PossibleSpamCommentDetected(PossibleSpamCommentEventArgs eventArgs)
        {
            emailSender.NotifySpamComment(eventArgs.DetailUrl, eventArgs.Comment, eventArgs.Source);
        }
    }
}