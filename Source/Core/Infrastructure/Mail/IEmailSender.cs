namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;
    using Service;

    public interface IEmailSender
    {
        void SendRegistrationInfo(string email, string userName, string password, string activateUrl);

        void SendNewPassword(string email, string userName, string password);

        void SendComment(string url, Comment comment, IEnumerable<User> users);

        void NotifySpamStory(string url, Story story, string source);

        void NotifyStoryMarkedAsSpam(string url, Story story, User byUser);

        void NotifySpamComment(string url, Comment comment, string source);

        void NotifyStoryApprove(string url, Story story, User byUser);

        void NotifyConfirmSpamStory(string url, Story story, User byUser);

        void NotifyConfirmSpamComment(string url, Comment comment, User byUser);

        void NotifyCommentAsOffended(string url, Comment comment, User byUser);

        void NotifyStoryDelete(Story story, User byUser);

        void NotifyPublishedStories(DateTime timestamp, IEnumerable<PublishedStory> stories);

        void NotifyFeedback(string email, string name, string content);
    }
}