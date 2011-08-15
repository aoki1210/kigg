namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class StoryDeleteEventArgs
    {
        [DebuggerStepThrough]
        public StoryDeleteEventArgs(Story story, User user)
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

    public class StoryDeleteEvent : BaseEvent<StoryDeleteEventArgs>
    {
    }
}