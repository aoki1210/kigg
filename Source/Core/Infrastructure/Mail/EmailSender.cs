namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using System.Threading;

    using Domain.Entities;
    using Service;

    public class EmailSender : IEmailSender
    {
        private readonly string _templateDirectory;
        private readonly bool _enableSsl;
        private readonly IConfigurationSettings _settings;
        private readonly IFile _file;

        public EmailSender(string templateDirectory, bool enableSsl, IConfigurationSettings settings, IFile file)
        {
            Check.Argument.IsNotNullOrEmpty(templateDirectory, "templateDirectory");
            Check.Argument.IsNotNull(settings, "settings");
            Check.Argument.IsNotNull(file, "file");

            _templateDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templateDirectory);
            _enableSsl = enableSsl;
            _settings = settings;
            _file = file;
        }

        public void SendRegistrationInfo(string email, string userName, string password, string activateUrl)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            Check.Argument.IsNotNullOrEmpty(userName, "userName");
            Check.Argument.IsNotNullOrEmpty(password, "password");
            Check.Argument.IsNotNullOrEmpty(activateUrl, "activateUrl");

            string body = PrepareMailBodyWith("Registration", "userName", userName, "password", password, "url", activateUrl);

            SendMailAsync(_settings.WebmasterEmail, email, "{0}: Registration Info".FormatWith(_settings.SiteTitle), body);
        }

        public void SendNewPassword(string email, string userName, string password)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            Check.Argument.IsNotNullOrEmpty(userName, "userName");
            Check.Argument.IsNotNullOrEmpty(password, "password");

            string body = PrepareMailBodyWith("NewPassword", "userName", userName, "password", password);

            SendMailAsync(_settings.WebmasterEmail, email, "{0}: Your new password".FormatWith(_settings.SiteTitle), body);
        }

        public void SendComment(string url, Comment comment, IEnumerable<User> users)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotNull(comment, "comment");
            Check.Argument.IsNotNull(users, "users");

            string body = PrepareMailBodyWith("NewComment", "userName", comment.ByUser.UserName, "detailUrl", url, "comment", comment.TextBody);
            string subject = "{0}: New Comment - {1}".FormatWith(_settings.SiteTitle, comment.ForStory.Title);

            ThreadPool.QueueUserWorkItem(state =>
                                                    {
                                                         foreach (User user in users)
                                                         {
                                                             SendMail(_settings.WebmasterEmail, user.Email, subject, body);
                                                         }
                                                    });
        }

        public void NotifySpamStory(string url, Story story, string source)
        {
            Check.Argument.IsNotNullOrEmpty(url, "storyUrl");
            Check.Argument.IsNotNull(story, "story");
            Check.Argument.IsNotNullOrEmpty(source, "source");

            string body = PrepareMailBodyWith("SpamStory", "title", story.Title, "siteUrl", url, "originalUrl", story.Url, "userName", story.PostedBy.UserName, "protection", source);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Spam Story Submitted - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifyStoryMarkedAsSpam(string url, Story story, User byUser)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotNull(story, "story");
            Check.Argument.IsNotNull(byUser, "byUser");

            string body = PrepareMailBodyWith("MarkAsSpamStory", "markedByUserName", byUser.UserName, "title", story.Title, "siteUrl", url, "originalUrl", story.Url, "postedByUserName", story.PostedBy.UserName);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Story Marked As Spam - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifySpamComment(string url, Comment comment, string source)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotNull(comment, "comment");
            Check.Argument.IsNotNullOrEmpty(source, "source");

            string body = PrepareMailBodyWith("SpamComment", "siteUrl", url, "userName", comment.ByUser.UserName, "comment", comment.HtmlBody, "protection", source);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Spam Comment Submitted - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifyStoryApprove(string url, Story story, User byUser)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotNull(story, "story");
            Check.Argument.IsNotNull(byUser, "byUser");

            string body = PrepareMailBodyWith("ApproveStory", "approvedByUserName", byUser.UserName, "title", story.Title, "siteUrl", url, "originalUrl", story.Url, "postedByUserName", story.PostedBy.UserName);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Story Approved - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifyConfirmSpamStory(string url, Story story, User byUser)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotNull(story, "story");
            Check.Argument.IsNotNull(byUser, "byUser");

            string postedBy = (story.PostedBy == null) ? String.Empty : story.PostedBy.UserName;

            string body = PrepareMailBodyWith("ConfirmSpamStory", "confirmedByUserName", byUser.UserName, "title", story.Title, "siteUrl", url, "originalUrl", story.Url, "postedByUserName", postedBy);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Story Confirmed As Spam - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifyConfirmSpamComment(string url, Comment comment, User byUser)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotNull(comment, "comment");
            Check.Argument.IsNotNull(byUser, "byUser");

            string body = PrepareMailBodyWith("ConfirmSpamComment", "confirmedByUserName", byUser.UserName, "siteUrl", url, "postedByUserName", comment.ByUser.UserName, "comment", comment.HtmlBody);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Comment Confirmed As Spam - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifyCommentAsOffended(string url, Comment comment, User byUser)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotNull(comment, "comment");
            Check.Argument.IsNotNull(byUser, "byUser");

            string body = PrepareMailBodyWith("OffendedComment", "offendedByUserName", byUser.UserName, "siteUrl", url, "postedByUserName", comment.ByUser.UserName, "comment", comment.HtmlBody);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Comment Marked As Offended - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifyStoryDelete(Story story, User byUser)
        {
            Check.Argument.IsNotNull(story, "story");
            Check.Argument.IsNotNull(byUser, "byUser");

            string postedBy = (story.PostedBy == null) ? String.Empty : story.PostedBy.UserName;

            string body = PrepareMailBodyWith("StoryDeleted", "deletedByUserName", byUser.UserName, "title", story.Title, "url", story.Url, "postedByUserName", postedBy);

            SendMailAsync(_settings.WebmasterEmail, _settings.SupportEmail, "Story Deleted - {0}".FormatWith(SystemTime.Now().ToLongDateString()), body);
        }

        public void NotifyPublishedStories(DateTime timestamp, IEnumerable<PublishedStory> stories)
        {
            Check.Argument.IsNotInFuture(timestamp, "timestamp");
            Check.Argument.IsNotNull(stories, "stories");

            string subject = "{0}: Published Stories - {1}".FormatWith(_settings.SiteTitle, timestamp.ToString());
            string body = PrepareMailBodyWith("PublishedStories", "timestamp", timestamp.ToLongDateString());

            StringBuilder bodyBuilder = new StringBuilder(body);

            bool isFirstColumn = true;

            if (stories.Count() > 0)
            {
                // First Dump the Column names
                foreach (KeyValuePair<string, double> pair in stories.ElementAtOrDefault(0).Weights)
                {
                    if (!isFirstColumn)
                    {
                        bodyBuilder.Append(",");
                    }

                    bodyBuilder.Append("\"{0}\"".FormatWith(pair.Key));
                    isFirstColumn = false;
                }

                bodyBuilder.Append(",\"Total\",\"Rank\",\"Category\",\"Title\",\"Url\"");
                bodyBuilder.AppendLine();

                // The the values
                foreach (PublishedStory ps in stories)
                {
                    isFirstColumn = true;

                    foreach (KeyValuePair<string, double> pair in ps.Weights)
                    {
                        if (!isFirstColumn)
                        {
                            bodyBuilder.Append(",");
                        }

                        bodyBuilder.Append("\"{0}\"".FormatWith(pair.Value));
                        isFirstColumn = false;
                    }

                    bodyBuilder.Append(",\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"".FormatWith(ps.TotalScore, ps.Rank, ps.Story.BelongsTo.Name, ps.Story.Title, ps.Story.Url));
                    bodyBuilder.AppendLine();
                }
            }

            SendMail(_settings.WebmasterEmail, _settings.SupportEmail, subject, bodyBuilder.ToString());
        }

        public void NotifyFeedback(string email, string name, string content)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            Check.Argument.IsNotNullOrEmpty(name, "name");
            Check.Argument.IsNotNullOrEmpty(content, "content");

            string body = PrepareMailBodyWith("Feedback", "name", name, "email", email, "content", content);

            SendMailAsync(email, _settings.SupportEmail, "Feedbaack from {0}".FormatWith(name), body);
        }

        private string PrepareMailBodyWith(string templateName, params string[] pairs)
        {
            string body = GetMailBodyOfTemplate(templateName);

            for (var i = 0; i < pairs.Length; i += 2)
            {
                body = body.Replace("<%={0}%>".FormatWith(pairs[i]), pairs[i + 1]);
            }

            body = body.Replace("<%=siteTitle%>", _settings.SiteTitle);
            body = body.Replace("<%=rootUrl%>", _settings.RootUrl);

            return body;
        }

        private string GetMailBodyOfTemplate(string templateName)
        {
            string cacheKey = string.Concat("mailTemplate:", templateName);
            string body;

            Cache.TryGet(cacheKey, out body);

            if (string.IsNullOrEmpty(body))
            {
                body = ReadFileFrom(templateName);

                if ((!string.IsNullOrEmpty(body)) && (!Cache.Contains(cacheKey)))
                {
                    Cache.Set(cacheKey, body);
                }
            }

            return body;
        }

        private string ReadFileFrom(string templateName)
        {
            string filePath = string.Concat(Path.Combine(_templateDirectory, templateName), ".txt");

            string body = _file.ReadAllText(filePath);

            return body;
        }

        private void SendMail(string fromAddress, string toAddress, string subject, string body)
        {
            if (string.Compare(toAddress, _settings.DefaultEmailOfOpenIdUser, StringComparison.OrdinalIgnoreCase) != 0)
            {
                using (MailMessage mail = BuildMessageWith(fromAddress, toAddress, subject, body))
                {
                    SendMail(mail);
                }
            }
        }

        private void SendMail(MailMessage mail)
        {
            try
            {
                SmtpClient smtp = new SmtpClient
                                      {
                                          EnableSsl = _enableSsl
                                      };

                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        private void SendMailAsync(string fromAddress, string toAddress, string subject, string body)
        {
            ThreadPool.QueueUserWorkItem(state => SendMail(fromAddress, toAddress, subject, body));
        }

        private MailMessage BuildMessageWith(string fromAddress, string toAddress, string subject, string body)
        {
            MailMessage message = new MailMessage
                                      {
                                            Sender = new MailAddress(_settings.WebmasterEmail), // on Behave of When From differs
                                            From = new MailAddress(fromAddress),
                                            Subject = subject,
                                            Body = body,
                                            IsBodyHtml = false,
                                      };

            string[] tos = toAddress.Split(';');

            foreach (string to in tos)
            {
                message.To.Add(new MailAddress(to));
            }

            return message;
        }
    }
}