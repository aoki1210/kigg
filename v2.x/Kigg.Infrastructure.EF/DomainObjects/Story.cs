namespace Kigg.EF.DomainObjects
{
    using System.Collections.Generic;

    using Kigg.DomainObjects;
    
    public partial class Story : IStory
    {
        public ICategory BelongsTo
        {
            get { throw new System.NotImplementedException(); }
        }

        public IUser PostedBy
        {
            get { throw new System.NotImplementedException(); }
        }

        public string FromIPAddress
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICollection<IVote> Votes
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICollection<IMarkAsSpam> MarkAsSpams
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICollection<IStoryView> Views
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICollection<IComment> Comments
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICollection<IUser> Subscribers
        {
            get { throw new System.NotImplementedException(); }
        }

        public int VoteCount
        {
            get { throw new System.NotImplementedException(); }
        }

        public int MarkAsSpamCount
        {
            get { throw new System.NotImplementedException(); }
        }

        public int ViewCount
        {
            get { throw new System.NotImplementedException(); }
        }

        public int CommentCount
        {
            get { throw new System.NotImplementedException(); }
        }

        public int SubscriberCount
        {
            get { throw new System.NotImplementedException(); }
        }

        public void ChangeCategory(ICategory category)
        {
            throw new System.NotImplementedException();
        }

        public void View(System.DateTime at, string fromIpAddress)
        {
            throw new System.NotImplementedException();
        }

        public bool CanPromote(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public bool Promote(System.DateTime at, IUser byUser, string fromIpAddress)
        {
            throw new System.NotImplementedException();
        }

        public bool HasPromoted(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public bool CanDemote(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public bool Demote(System.DateTime at, IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public bool CanMarkAsSpam(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public bool MarkAsSpam(System.DateTime at, IUser byUser, string fromIpAddress)
        {
            throw new System.NotImplementedException();
        }

        public bool HasMarkedAsSpam(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public bool CanUnmarkAsSpam(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public bool UnmarkAsSpam(System.DateTime at, IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public IComment PostComment(string content, System.DateTime at, IUser byUser, string fromIpAddress)
        {
            throw new System.NotImplementedException();
        }

        public IComment FindComment(System.Guid id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteComment(IComment comment)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsCommentSubscriber(IUser theUser)
        {
            throw new System.NotImplementedException();
        }

        public void SubscribeComment(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public void UnsubscribeComment(IUser byUser)
        {
            throw new System.NotImplementedException();
        }

        public void Approve(System.DateTime at)
        {
            throw new System.NotImplementedException();
        }

        public void Publish(System.DateTime at, int rank)
        {
            throw new System.NotImplementedException();
        }

        public void LastProcessed(System.DateTime at)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeNameAndCreatedAt(string name, System.DateTime createdAt)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<ITag> Tags
        {
            get { throw new System.NotImplementedException(); }
        }

        public int TagCount
        {
            get { throw new System.NotImplementedException(); }
        }

        public void AddTag(ITag tag)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTag(ITag tag)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAllTags()
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsTag(ITag tag)
        {
            throw new System.NotImplementedException();
        }
    }
}
