namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public partial class StoryView : IStoryView
    {
        public IStory ForStory
        {
            get { throw new System.NotImplementedException(); }
        }

        public string FromIPAddress
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.DateTime ViewedAt
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
