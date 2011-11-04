namespace Kigg.Service
{
    using System.Diagnostics;
    using Domain.Entities;
    using Infrastructure;

    public class StoryUnmarkAsSpamEventArgs
    {
        [DebuggerStepThrough]
        public StoryUnmarkAsSpamEventArgs(Story story, User user)
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

    public class StoryUnmarkAsSpamEvent : BaseEvent<StoryUnmarkAsSpamEventArgs>
    {
    }
}