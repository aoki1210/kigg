using System;

namespace Kigg.Infrastructure.DomainRepositoryExtensions
{
    using System.Security.Permissions;

    using Repository;
    using DomainObjects;
    
    [StrongNameIdentityPermissionAttribute(SecurityAction.Demand, PublicKey = "00240000048000009400000006020000002400005253413100040000010001007f9d35f7398744b708ea57288eb1911f9a46cad961be6baacb27e07d87809a20bf135f61833c121b541676fa95fd373d44ac4404ffae85e5199d0828c00991362b34f93002791f16d901f1714ba3abaa9208f8c41660f57ae0e7732e3655d5d4d9c53521cdb1b0636a78aac7407e194b7bee1a45b229e35559ee6c0a5b11b5b9")]
    public static class StoryExtension
    {
        public static int GetViewCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IStoryViewRepository>(forStory.Id);
        }

        public static int GetVoteCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IVoteRepository>(forStory.Id);
        }

        public static int GetMarkAsSpamCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<IMarkAsSpamRepository>(forStory.Id);
        }

        public static int GetCommentCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<ICommentRepository>(forStory.Id);
        }

        public static int GetSubscriberCount(this IStory forStory)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            return GetCount<ICommentSubscribtionRepository>(forStory.Id);
        }

        private static int GetCount<T>(Guid storyId) where T : class, ICountByStoryRepository
        {
            return IoC.Resolve<T>().CountByStory(storyId);
        }

        public static IStoryView AddView(this IStory forStory, DateTime at, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var view = IoC.Resolve<IDomainObjectFactory>().CreateStoryView(forStory, at, fromIpAddress);
            IoC.Resolve<IStoryViewRepository>().Add(view);
            
            return view;
        }

        public static IVote AddVote(this IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var vote = IoC.Resolve<IDomainObjectFactory>().CreateStoryVote(forStory, at, byUser, fromIpAddress);
            IoC.Resolve<IVoteRepository>().Add(vote);

            return vote;
        }

        public static IVote RemoveVote(this IStory fromStory, DateTime at, IUser byUser)
        {
            Check.Argument.IsNotNull(fromStory, "fromStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");

            var repository = IoC.Resolve<IVoteRepository>();
            var vote = repository.FindById(fromStory.Id, byUser.Id);

            repository.Remove(vote);
                
            return vote;
        }
    }
}
