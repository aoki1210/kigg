namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
    using Infrastructure;

    public class StoryApproveEventArgs
    {
        [DebuggerStepThrough]
        public StoryApproveEventArgs(Story story, User user, string detailUrl)
        {
            Story = story;
            User = user;
            DetailUrl = detailUrl;
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

        public string DetailUrl
        {
            get;
            private set;
        }
    }

    public class StoryApproveEvent : BaseEvent<StoryApproveEventArgs>
    {
    }
}