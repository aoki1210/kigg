
namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryComment : IComment
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

        public void MarkAsOffended()
        {
            IsOffended = true;
        }
    }
}
