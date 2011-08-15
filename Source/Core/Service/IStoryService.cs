namespace Kigg.Service
{
    using System;
    using System.Collections.Specialized;

    using DomainObjects;

    public interface IStoryService
    {
        StoryCreateResult Create(User byUser, string url, string title, string category, string description, string tags, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables, Func<Story, string> buildDetailUrl);

        void Update(Story theStory, string uniqueName, DateTime createdAt, string title, string category, string description, string tags);

        void Delete(Story theStory, User byUser);

        void View(Story theStory, User byUser, string fromIPAddress);

        void Promote(Story theStory, User byUser, string fromIPAddress);

        void Demote(Story theStory, User byUser);

        void MarkAsSpam(Story theStory, string storyUrl, User byUser, string fromIPAddress);

        void UnmarkAsSpam(Story theStory, User byUser);

        CommentCreateResult Comment(Story forStory, string storyUrl, User byUser, string content, bool subscribe, string userIPAddress, string userAgent, string urlReferer, NameValueCollection serverVariables);

        void Publish();

        void Approve(Story theStory, string storyUrl, User byUser);

        void Spam(Story theStory, string storyUrl, User byUser);

        void Spam(Comment theComment, string storyUrl, User byUser);

        void MarkAsOffended(Comment theComment, string storyUrl, User byUser);
    }
}