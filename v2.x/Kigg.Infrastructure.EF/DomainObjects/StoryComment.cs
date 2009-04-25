
namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryComment : IComment
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

        public void MarkAsOffended()
        {
            throw new System.NotImplementedException();
        }
    }
}
