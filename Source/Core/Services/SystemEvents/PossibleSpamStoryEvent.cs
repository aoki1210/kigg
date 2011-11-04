namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
    using Infrastructure;

    public class PossibleSpamStoryEventArgs
    {
        [DebuggerStepThrough]
        public PossibleSpamStoryEventArgs(Story story, string source, string detailUrl)
        {
            Story = story;
            Source = source;
            DetailUrl = detailUrl;
        }

        public Story Story
        {
            get;
            private set;
        }

        public string Source
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

    public class PossibleSpamStoryEvent : BaseEvent<PossibleSpamStoryEventArgs>
    {
    }
}