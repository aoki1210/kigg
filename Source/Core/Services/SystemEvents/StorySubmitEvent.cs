namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
    using Infrastructure;

    public class StorySubmitEventArgs
    {
        [DebuggerStepThrough]
        public StorySubmitEventArgs(Story story, string detailUrl)
        {
            Story = story;
            DetailUrl = detailUrl;
        }

        public Story Story
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

    public class StorySubmitEvent : BaseEvent<StorySubmitEventArgs>
    {
    }
}