namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
    using Infrastructure;

    public class CommentSubmitEventArgs
    {
        [DebuggerStepThrough]
        public CommentSubmitEventArgs(Comment comment, string detailUrl)
        {
            Comment = comment;
            DetailUrl = detailUrl;
        }

        public Comment Comment
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

    public class CommentSubmitEvent : BaseEvent<CommentSubmitEventArgs>
    {
    }
}