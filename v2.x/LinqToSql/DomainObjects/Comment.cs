namespace Kigg.DomainObjects
{
    using System;

    public partial class StoryComment : IComment
    {
        public IUser ByUser
        {
            get
            {
                return User;
            }
        }

        public string FromIPAddress
        {
            get
            {
                return IPAddress;
            }
        }

        public DateTime PostedAt
        {
            get
            {
                return Timestamp;
            }
        }

        public IStory ForStory
        {
            get
            {
                return Story;
            }
        }

        public virtual void MarkAsOffended()
        {
            IsOffended = true;
        }
    }
}