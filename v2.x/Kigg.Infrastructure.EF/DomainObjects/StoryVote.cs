namespace Kigg.EF.DomainObjects
{
    using System;

    using Kigg.DomainObjects;

    public partial class StoryVote : IVote
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

        public DateTime PromotedAt
        {
            get
            {
                return Timestamp;
            }
        }
    }
}
