namespace Kigg.Infrastructure.DomainRepositoryExtensions
{
    using System;
    using System.Diagnostics;
    using System.Security.Permissions;

    using Repository;
    using Domain.Entities;
    
    //[StrongNameIdentityPermissionAttribute(SecurityAction.Demand, PublicKey = "00240000048000009400000006020000002400005253413100040000010001007f9d35f7398744b708ea57288eb1911f9a46cad961be6baacb27e07d87809a20bf135f61833c121b541676fa95fd373d44ac4404ffae85e5199d0828c00991362b34f93002791f16d901f1714ba3abaa9208f8c41660f57ae0e7732e3655d5d4d9c53521cdb1b0636a78aac7407e194b7bee1a45b229e35559ee6c0a5b11b5b9")]
    public static class StoryExtension
    {
        [DebuggerStepThrough]
        public static int GetViewCount(this Story forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IStoryViewRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetVoteCount(this Story forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IVoteRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetMarkAsSpamCount(this Story forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<ISpamVoteRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetCommentCount(this Story forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<ICommentRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        public static int GetSubscriberCount(this Story forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<ICommentSubscribtionRepository>(forStory.Id);
        }

        [DebuggerStepThrough]
        private static int GetCount<T>(long storyId) where T : class, ICountByStoryRepository
        {
            return IoC.Resolve<T>().CountByStory(storyId);
        }

        [DebuggerStepThrough]
        public static StoryView AddView(this Story forStory, DateTime at, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotNullOrEmpty(fromIpAddress, "fromIpAddress");

            var view = IoC.Resolve<IDomainObjectFactory>().CreateStoryView(forStory, at, fromIpAddress);
            IoC.Resolve<IStoryViewRepository>().Add(view);
            
            return view;
        }

        [DebuggerStepThrough]
        public static Vote AddVote(this Story forStory, DateTime at, User byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotNullOrEmpty(fromIpAddress, "fromIpAddress");

            var vote = IoC.Resolve<IDomainObjectFactory>().CreateStoryVote(forStory, at, byUser, fromIpAddress);
            IoC.Resolve<IVoteRepository>().Add(vote);

            return vote;
        }

        [DebuggerStepThrough]
        public static void RemoveVote(this Story fromStory, Vote vote)
        {
            Check.Argument.IsNotNull(fromStory, "fromStory");
            Check.Argument.IsNotNull(vote, "vote");
            IoC.Resolve<IVoteRepository>().Remove(vote);            
        }

        [DebuggerStepThrough]
        public static Vote GetVote(this Story forStory,User byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            return IoC.Resolve<IVoteRepository>().FindById(forStory.Id, byUser.Id);
        }

        [DebuggerStepThrough]
        public static SpamVote MarkSpam(this Story forStory, DateTime at, User byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotNullOrEmpty(fromIpAddress, "fromIpAddress");

            var spamStory = IoC.Resolve<IDomainObjectFactory>().CreateMarkAsSpam(forStory, at, byUser, fromIpAddress);
            IoC.Resolve<ISpamVoteRepository>().Add(spamStory);
            
            return spamStory;
        }

        [DebuggerStepThrough]
        public static void UnmarkSpam(this Story forStory, SpamVote spam)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(spam, "spam");
            IoC.Resolve<ISpamVoteRepository>().Remove(spam);
        }

        [DebuggerStepThrough]
        public static SpamVote GetMarkAsSpam(this Story forStory, User byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");
            return IoC.Resolve<ISpamVoteRepository>().FindById(forStory.Id, byUser.Id);
        }

        [DebuggerStepThrough]
        public static Comment AddComment(this Story forStory, string content, DateTime at, User byUser, string fromIpAddress)
        {
            var comment = IoC.Resolve<IDomainObjectFactory>().CreateComment(forStory, content, at, byUser, fromIpAddress);
            IoC.Resolve<ICommentRepository>().Add(comment);
            return comment;
        }

        [DebuggerStepThrough]
        public static Comment GetComment(this Story forStory, long commentId)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            //Check.Argument.IsNotEmpty(commentId, "commentId");
            return IoC.Resolve<ICommentRepository>().FindById(commentId);
        }

        [DebuggerStepThrough]
        public static void RemoveComment(this Story forStory, Comment comment)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(comment, "comment");
            IoC.Resolve<ICommentRepository>().Remove(comment);
        }

        [DebuggerStepThrough]
        public static CommentSubscribtion GetCommentSubscribtion(this Story forStory, User theUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(theUser, "theUser");
            return IoC.Resolve<ICommentSubscribtionRepository>().FindById(forStory.Id, theUser.Id);
        }

        [DebuggerStepThrough]
        public static CommentSubscribtion AddCommentSubscribtion(this Story forStory, User byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            var subscribtion = forStory.GetCommentSubscribtion(byUser);

            if (subscribtion == null)
            {
                subscribtion = IoC.Resolve<IDomainObjectFactory>().CreateCommentSubscribtion(forStory, byUser);
                IoC.Resolve<ICommentSubscribtionRepository>().Add(subscribtion);
            }
            return subscribtion;
        }

        [DebuggerStepThrough]
        public static CommentSubscribtion RemoveCommentSubscribtion(this Story forStory, User byUser)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            var subscribtion = forStory.GetCommentSubscribtion(byUser);

            if (subscribtion != null)
            {
                IoC.Resolve<ICommentSubscribtionRepository>().Remove(subscribtion);
            }
            return subscribtion;
        }

    }
}
