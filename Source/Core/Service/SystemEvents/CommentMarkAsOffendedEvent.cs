namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class CommentMarkAsOffendedEventArgs
    {
        [DebuggerStepThrough]
        public CommentMarkAsOffendedEventArgs(Comment comment, User user, string detailUrl)
        {
            Comment = comment;
            User = user;
            DetailUrl = detailUrl;
        }

        public Comment Comment
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

    public class CommentMarkAsOffendedEvent : BaseEvent<CommentMarkAsOffendedEventArgs>
    {
    }
}