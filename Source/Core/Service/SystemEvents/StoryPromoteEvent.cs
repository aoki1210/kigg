namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class StoryPromoteEventArgs
    {
        [DebuggerStepThrough]
        public StoryPromoteEventArgs(Story story, User user)
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

    public class StoryPromoteEvent : BaseEvent<StoryPromoteEventArgs>
    {
    }
}