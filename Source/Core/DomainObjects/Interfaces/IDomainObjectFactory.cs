namespace Kigg.DomainObjects
{
    using System;

    public interface IDomainObjectFactory
    {
        User CreateUser(string userName, string email, string password);

        KnownSource CreateKnownSource(string url);

        Category CreateCategory(string name);

        Tag CreateTag(string name);

        Story CreateStory(Category forCategory, User byUser, string fromIpAddress, string title, string description, string url);

        StoryView CreateStoryView(Story forStory, DateTime at, string fromIpAddress);

        Vote CreateStoryVote(Story forStory, DateTime at, User byUser, string fromIpAddress);

        MarkAsSpam CreateMarkAsSpam(Story forStory, DateTime at, User byUser, string fromIpAddress);

        Comment CreateComment(Story forStory, string content, DateTime at, User byUser, string fromIpAddress);

        CommentSubscribtion CreateCommentSubscribtion(Story forStory, User byUser);
    }
}