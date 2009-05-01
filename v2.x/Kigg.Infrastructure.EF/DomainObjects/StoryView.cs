using System;

namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryView : IStoryView
    {
        public IStory ForStory
        {
            get
            {
                return Story;
            }
        }

        public string FromIPAddress
        {
            get
            {
                return IpAddress;
            }
        }

        public DateTime ViewedAt
        {
            get
            {
                return Timestamp;
            }
        }
    }
}
