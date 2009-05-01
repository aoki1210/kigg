using System;

namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryMarkAsSpam : IMarkAsSpam
    {
        public IStory ForStory
        {
            get
            {
                return Story;
            }
        }

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
                return IpAddress;
            }
        }

        public DateTime MarkedAt
        {
            get
            {
                return Timestamp;
            }
        }
    }
}
