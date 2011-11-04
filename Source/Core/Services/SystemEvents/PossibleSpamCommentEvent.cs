namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
    using Infrastructure;

    public class PossibleSpamCommentEventArgs
    {
        [DebuggerStepThrough]
        public PossibleSpamCommentEventArgs(Comment comment, string source, string detailUrl)
        {
            Comment = comment;
            Source = source;
            DetailUrl = detailUrl;
        }

        public Comment Comment
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

    public class PossibleSpamCommentEvent : BaseEvent<PossibleSpamCommentEventArgs>
    {
    }
}