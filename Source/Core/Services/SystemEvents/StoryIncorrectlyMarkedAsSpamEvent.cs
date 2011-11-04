namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
    using Infrastructure;

    public class StoryIncorrectlyMarkedAsSpamEventArgs
    {
        [DebuggerStepThrough]
        public StoryIncorrectlyMarkedAsSpamEventArgs(Story story, User user)
        {
            Story = story;
            User = user;
        }

        public Story Story
        {
            get;
            private set;
        }

        public User User
        {
            get;
            private set;
        }
    }

    public class StoryIncorrectlyMarkedAsSpamEvent : BaseEvent<StoryIncorrectlyMarkedAsSpamEventArgs>
    {
    }
}