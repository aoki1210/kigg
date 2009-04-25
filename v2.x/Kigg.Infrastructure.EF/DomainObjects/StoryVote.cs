namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryVote : IVote
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

        public System.DateTime PromotedAt
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
