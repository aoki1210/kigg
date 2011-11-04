namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
    using Infrastructure;

    public class StoryViewEventArgs
    {
        [DebuggerStepThrough]
        public StoryViewEventArgs(Story story, User user)
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

    public class StoryViewEvent : BaseEvent<StoryViewEventArgs>
    {
    }
}