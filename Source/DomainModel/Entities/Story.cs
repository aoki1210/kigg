namespace Kigg.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class Story : IUniqueNameEntity, ITagContainer
    {
        public virtual long Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string UniqueName { get; set; }

        public virtual string HtmlDescription { get; set; }

        public virtual string TextDescription { get; set; }

        public virtual string Url { get; set; }

        public virtual string UrlHash { get; set; }

        public virtual Category BelongsTo { get; set; }

        public virtual User PostedBy { get; set; }

        public virtual string FromIPAddress { get; set; }

        public virtual int? Rank { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual DateTime LastActivityAt { get; set; }

        public virtual DateTime? ApprovedAt { get; set; }

        public virtual DateTime? PublishedAt { get; set; }

        public virtual DateTime? LastProcessedAt { get; set; }

        protected internal virtual ICollection<Comment> Comments
        {
            get;
            set;
        }

        protected internal virtual ICollection<Vote> Votes
        {
            get;
            set;
        }

        protected internal virtual ICollection<SpamVote> MarkAsSpams
        {
            get;
            set;
        }

        protected virtual ICollection<StoryView> Views
        {
            get;
            set;
        }
        
        public virtual ICollection<Tag> Tags { get; protected set; }

        public int VoteCount
        {
            get { throw new NotImplementedException(); }

        }

        public int MarkAsSpamCount
        {
            get { throw new NotImplementedException(); }
        }

        public int ViewCount
        {
            get { throw new NotImplementedException(); }
        }

        public int CommentCount
        {
            get { throw new NotImplementedException(); }
        }

        public void AddTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public void RemoveTag(Tag tag)
        {
            throw new NotImplementedException();
        }
        
        public bool ContainsTag(Tag tag)
        {
            throw new NotImplementedException();
        }

        public void ChangeCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public void View(DateTime at, string fromIpAddress)
        {
            throw new NotImplementedException();
        }

        public bool CanPromote(User byUser)
        {
            throw new NotImplementedException();
        }

        public bool Promote(DateTime at, User byUser, string fromIpAddress)
        {
            throw new NotImplementedException();
        }

        public bool HasPromoted(User byUser)
        {
            throw new NotImplementedException();
        }

        public bool CanDemote(User byUser)
        {
            throw new NotImplementedException();
        }

        public bool Demote(DateTime at, User byUser)
        {
            throw new NotImplementedException();
        }

        public bool CanMarkAsSpam(User byUser)
        {
            throw new NotImplementedException();
        }

        public bool MarkAsSpam(DateTime at, User byUser, string fromIpAddress)
        {
            throw new NotImplementedException();
        }

        public bool HasMarkedAsSpam(User byUser)
        {
            throw new NotImplementedException();
        }

        public bool CanUnmarkAsSpam(User byUser)
        {
            throw new NotImplementedException();
        }

        public bool UnmarkAsSpam(DateTime at, User byUser)
        {
            throw new NotImplementedException();
        }

        public Comment PostComment(string content, DateTime at, User byUser, string fromIpAddress)
        {
            throw new NotImplementedException();
        }

        public Comment FindComment(long id)
        {
            throw new NotImplementedException();
        }

        public void DeleteComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public bool ContainsCommentSubscriber(User theUser)
        {
            throw new NotImplementedException();
        }

        public void SubscribeComment(User byUser)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeComment(User byUser)
        {
            throw new NotImplementedException();
        }

        public void Approve(DateTime at)
        {
            throw new NotImplementedException();
        }

        public void Publish(DateTime at, int rank)
        {
            throw new NotImplementedException();
        }

        public void LastProcessed(DateTime at)
        {
            throw new NotImplementedException();
        }

        public void ChangeNameAndCreatedAt(string name, DateTime createdAt)
        {
            throw new NotImplementedException();
        }

        public bool HasTags()
        {
            throw new NotImplementedException();
        }

        public bool HasComments()
        {
            throw new NotImplementedException();
        }
    }
}