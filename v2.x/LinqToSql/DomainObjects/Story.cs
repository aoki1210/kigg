namespace Kigg.LinqToSql.DomainObjects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Kigg.DomainObjects;
    using Kigg.Repository;
    using Infrastructure;
    using Infrastructure.DomainRepositoryExtensions;
    
    public partial class Story : IStory
    {
        private const int NotSet = -1;

        private int _voteCount = NotSet;
        private int _markAsSpamCount = NotSet;
        private int _viewCount = NotSet;
        private int _commentCount = NotSet;
        private int _subscriberCount = NotSet;

        public ICategory BelongsTo
        {
            [DebuggerStepThrough]
            get
            {
                return Category;
            }
        }

        public IUser PostedBy
        {
            [DebuggerStepThrough]
            get
            {
                return User;
            }
        }

        public string FromIPAddress
        {
            [DebuggerStepThrough]
            get
            {
                return IPAddress;
            }
        }

        public ICollection<ITag> Tags
        {
            [DebuggerStepThrough]
            get
            {
                return StoryTags.Select(st => st.Tag).OrderBy(t => t.Name).Cast<ITag>().ToList().AsReadOnly();
            }
        }

        public ICollection<IVote> Votes
        {
            [DebuggerStepThrough]
            get
            {
                return StoryVotes.OrderBy(v => v.Timestamp).Cast<IVote>().ToList().AsReadOnly();
            }
        }

        public ICollection<IMarkAsSpam> MarkAsSpams
        {
            [DebuggerStepThrough]
            get
            {
                return StoryMarkAsSpams.OrderBy(s => s.Timestamp).Cast<IMarkAsSpam>().ToList().AsReadOnly();
            }
        }

        public ICollection<IStoryView> Views
        {
            [DebuggerStepThrough]
            get
            {
                return StoryViews.OrderBy(v => v.Timestamp).Cast<IStoryView>().ToList().AsReadOnly();
            }
        }

        public ICollection<IComment> Comments
        {
            [DebuggerStepThrough]
            get
            {
                return StoryComments.OrderBy(c => c.CreatedAt).Cast<IComment>().ToList().AsReadOnly();
            }
        }

        public ICollection<IUser> Subscribers
        {
            [DebuggerStepThrough]
            get
            {
                return CommentSubscribtions.Select(cs => cs.User).Cast<IUser>().ToList().AsReadOnly();
            }
        }

        public int TagCount
        {
            [DebuggerStepThrough]
            get
            {
                return StoryTags.Count();
            }
        }

        public int VoteCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_voteCount == NotSet)
                {
                    _voteCount = this.GetVoteCount();
                }

                return _voteCount;
            }
        }

        public int MarkAsSpamCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_markAsSpamCount == NotSet)
                {
                    _markAsSpamCount = this.GetMarkAsSpamCount();
                }

                return _markAsSpamCount;
            }
        }

        public int ViewCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_viewCount == NotSet)
                {
                    _viewCount = this.GetViewCount();
                }

                return _viewCount;
            }
        }

        public int CommentCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_commentCount == NotSet)
                {
                    _commentCount = this.GetCommentCount();
                }

                return _commentCount;
            }
        }

        public int SubscriberCount
        {
            [DebuggerStepThrough]
            get
            {
                if (_subscriberCount == NotSet)
                {
                    _subscriberCount = this.GetSubscriberCount();
                }

                return _subscriberCount;
            }
        }

        public virtual void ChangeCategory(ICategory category)
        {
            Check.Argument.IsNotNull(category, "category");

            Category = (Category) category;
        }

        public virtual void AddTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Id, "tag.Id");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            if (!ContainsTag(tag))
            {
                StoryTags.Add(new StoryTag { Tag = (Tag) tag });
            }
        }

        public virtual void RemoveTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            StoryTags.Remove(StoryTags.SingleOrDefault(st => st.Tag.Name == tag.Name));
        }

        public virtual void RemoveAllTags()
        {
            StoryTags.Clear();
        }

        public virtual bool ContainsTag(ITag tag)
        {
            Check.Argument.IsNotNull(tag, "tag");
            Check.Argument.IsNotEmpty(tag.Name, "tag.Name");

            return StoryTags.Any(st => st.Tag.Name == tag.Name);
        }

        public virtual void View(DateTime at, string fromIpAddress)
        {
            //Call extension method AddView, it will perform all parameters validation checks
            var view = this.AddView(at, fromIpAddress);

            //Add created view to StoryViews, this should increment views
            StoryViews.Add((StoryView)view);
            
            LastActivityAt = at;
        }

        public virtual bool CanPromote(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to promote the Story if
            // 1. User has not previously promoted it.
            // 2. User has not previously marked it as spam.
            return !HasPromoted(byUser) && !HasMarkedAsSpam(byUser);
        }

        public virtual bool Promote(DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(byUser, "byUser");
            
            //Check if user can promote
            if (CanPromote(byUser))
            {
                //Call extension method AddVote, it will perform all parameters validation checks
                var vote = this.AddVote(at, byUser, fromIpAddress);

                //Add created vote to StoryVotes, this should increment votes
                StoryVotes.Add((StoryVote)vote);
                
                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public virtual bool HasPromoted(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            return IoC.Resolve<IVoteRepository>().FindById(Id, byUser.Id) != null;
        }

        public virtual bool CanDemote(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to demote the Story if
            // 1. Story is not posted by the same user
            // 1. User has previously promoted it.
            return !this.IsPostedBy(byUser) && HasPromoted(byUser);
        }

        public virtual bool Demote(DateTime at, IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanDemote(byUser))
            {
                var vote = this.RemoveVote(at, byUser);
                
                StoryVotes.Remove((StoryVote)vote);

                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public virtual bool CanMarkAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to mark as spam when
            // 1. When Story is not published
            // 2. Story is not posted by the same user
            // 3. User has not previously promoted it.
            // 4. User has not previously marked it as spam.
            return !this.IsPublished() && !this.IsPostedBy(byUser) && !HasPromoted(byUser) && !HasMarkedAsSpam(byUser);
        }

        public virtual bool MarkAsSpam(DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            if (CanMarkAsSpam(byUser))
            {
                StoryMarkAsSpam markAsSpam = new StoryMarkAsSpam
                                                 {
                                                     Story = this,
                                                     User = (User) byUser,
                                                     IPAddress = fromIpAddress,
                                                     Timestamp = at
                                                 };

                StoryMarkAsSpams.Add(markAsSpam);
                IoC.Resolve<IMarkAsSpamRepository>().Add(markAsSpam);

                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public virtual bool HasMarkedAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            return IoC.Resolve<IMarkAsSpamRepository>().FindById(Id, byUser.Id) != null;
        }

        public virtual bool CanUnmarkAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            // User will be able to unmark as spam when
            // 1. When Story is not published
            // 2. User has previously marked it as spam.
            return !this.IsPublished() && HasMarkedAsSpam(byUser);
        }

        public virtual bool UnmarkAsSpam(DateTime at, IUser byUser)
        {
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanUnmarkAsSpam(byUser))
            {
                IMarkAsSpamRepository repository = IoC.Resolve<IMarkAsSpamRepository>();

                StoryMarkAsSpam spam = (StoryMarkAsSpam) repository.FindById(Id, byUser.Id);
                repository.Remove(spam);
                StoryMarkAsSpams.Remove(spam);

                LastActivityAt = at;

                return true;
            }

            return false;
        }

        public virtual IComment PostComment(string content, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotEmpty(content, "content");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            StoryComment comment = new StoryComment
                                       {
                                           Id = Guid.NewGuid(),
                                           HtmlBody = content.Trim(),
                                           TextBody = content.StripHtml().Trim(),
                                           Story = this,
                                           User = (User) byUser,
                                           IPAddress = fromIpAddress,
                                           CreatedAt = at
                                       };

            StoryComments.Add(comment);
            IoC.Resolve<ICommentRepository>().Add(comment);

            LastActivityAt = at;

            return comment;
        }

        public virtual IComment FindComment(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return IoC.Resolve<ICommentRepository>().FindById(Id, id);
        }

        public virtual void DeleteComment(IComment comment)
        {
            Check.Argument.IsNotNull(comment, "comment");

            StoryComment storyComment = (StoryComment) comment;

            IoC.Resolve<ICommentRepository>().Remove(comment);
            StoryComments.Remove(storyComment);
        }

        public virtual bool ContainsCommentSubscriber(IUser theUser)
        {
            Check.Argument.IsNotNull(theUser, "theUser");

            return IoC.Resolve<ICommentSubscribtionRepository>().FindById(Id, theUser.Id) != null;
        }

        public virtual void SubscribeComment(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            ICommentSubscribtionRepository repository = IoC.Resolve<ICommentSubscribtionRepository>();
            CommentSubscribtion subscribtion = repository.FindById(Id, byUser.Id) as CommentSubscribtion;

            if (subscribtion == null)
            {
                subscribtion = new CommentSubscribtion
                                   {
                                       Story = this,
                                       User = (User) byUser
                                   };

                CommentSubscribtions.Add(subscribtion);
                repository.Add(subscribtion);
            }
        }

        public virtual void UnsubscribeComment(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            ICommentSubscribtionRepository repository = IoC.Resolve<ICommentSubscribtionRepository>();
            CommentSubscribtion subscribtion = repository.FindById(Id, byUser.Id) as CommentSubscribtion;

            if (subscribtion != null)
            {
                CommentSubscribtions.Remove(subscribtion);
                repository.Remove(subscribtion);
            }
        }

        public virtual void Approve(DateTime at)
        {
            if (!this.IsApproved())
            {
                ApprovedAt = at;
            }
        }

        public virtual void Publish(DateTime at, int rank)
        {
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNegativeOrZero(rank, "rank");

            PublishedAt = at;
            Rank = rank;
            LastActivityAt = at;
        }

        public virtual void LastProcessed(DateTime at)
        {
            Check.Argument.IsNotInFuture(at, "at");

            LastProcessedAt = at;
        }

        public virtual void ChangeNameAndCreatedAt(string name, DateTime createdAt)
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotInFuture(createdAt, "createdAt");

            UniqueName = name;
            CreatedAt = createdAt;
        }

        partial void OnHtmlDescriptionChanged()
        {
            TextDescription = HtmlDescription.StripHtml().Trim();
        }
    }
}