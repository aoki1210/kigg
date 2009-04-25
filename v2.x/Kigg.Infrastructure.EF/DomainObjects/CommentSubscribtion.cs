namespace Kigg.EF.DomainObjects
{
    using Kigg.DomainObjects;

    public class CommentSubscribtion : ICommentSubscribtion
    {
        public IStory ForStory
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public IUser ByUser
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
    }
}