namespace Kigg.DomainObjects
{
    using System;

    public interface IComment
    {
        Guid Id
        {
            get;
        }

        IStory ForStory
        {
            get;
        }

        string HtmlBody
        {
            get;
        }

        string TextBody
        {
            get;
        }

        IUser ByUser
        {
            get;
        }

        string FromIPAddress
        {
            get;
        }

        DateTime PostedAt
        {
            get;
        }

        bool IsOffended
        {
            get;
        }

        void MarkAsOffended();
    }
}