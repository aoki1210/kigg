namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class StorySpamEventArgs
    {
        [DebuggerStepThrough]
        public StorySpamEventArgs(IStory story, IUser user, string detailUrl)
        {
            Story = story;
            User = user;
            DetailUrl = detailUrl;
        }

        public IStory Story
        {
            get;
            private set;
        }

        public IUser User
        {
            get;
            private set;
        }

        public string DetailUrl
        {
            get;
            private set;
        }
    }

    public class StorySpamEvent : BaseEvent<StorySpamEventArgs>
    {
    }
}