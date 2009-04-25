
namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryMarkAsSpam : IMarkAsSpam
    {
        public IStory ForStory
        {
            get { throw new System.NotImplementedException(); }
        }

        public IUser ByUser
        {
            get { throw new System.NotImplementedException(); }
        }

        public string FromIPAddress
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.DateTime MarkedAt
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
