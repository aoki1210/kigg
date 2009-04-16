﻿namespace Kigg.LinqToSql.DomainObjects
{
    using System;

    using Kigg.DomainObjects;

    public class DomainObjectFactory : IDomainObjectFactory
    {
        public virtual IUser CreateUser(string userName, string email, string password)
        {
            Check.Argument.IsNotEmpty(userName, "userName");
            Check.Argument.IsNotInvalidEmail(email, "email");

            DateTime now = SystemTime.Now();

            string hashedPassword = null;

            if (!string.IsNullOrEmpty(password))
            {
                hashedPassword = password.Trim().Hash();
            }

            return new User
                       {
                           Id = Guid.NewGuid(),
                           UserName = userName.Trim(),
                           Email = email.ToLowerInvariant(),
                           Password = hashedPassword,
                           Role = Roles.User,
                           IsActive = string.IsNullOrEmpty(hashedPassword),
                           LastActivityAt = now,
                           CreatedAt = now,
                       };
        }

        public virtual IKnownSource CreateKnownSource(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            return new KnownSource
                       {
                           Url = url.ToLowerInvariant(),
                           Grade = KnownSourceGrade.None
                       };
        }

        public virtual ICategory CreateCategory(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return new Category
                       {
                           Id = Guid.NewGuid(),
                           UniqueName = name.Trim().ToLegalUrl(),
                           Name = name.Trim(),
                           CreatedAt = SystemTime.Now()
                       };
        }

        public virtual ITag CreateTag(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");

            return new Tag
                       {
                           Id = Guid.NewGuid(),
                           UniqueName = name.Trim().ToLegalUrl(),
                           Name = name.Trim(),
                           CreatedAt = SystemTime.Now()
                       };
        }

        public virtual IStory CreateStory(ICategory forCategory, IUser byUser, string fromIpAddress, string title, string description, string url)
        {
            Check.Argument.IsNotNull(forCategory, "forCategory");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");
            Check.Argument.IsNotEmpty(title, "title");
            Check.Argument.IsNotEmpty(description, "description");
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            DateTime now = SystemTime.Now();

            var story = new Story
                            {
                                Id = Guid.NewGuid(),
                                UniqueName = title.Trim().ToLegalUrl(),
                                Title = title.Trim(),
                                HtmlDescription = description.Trim(),
                                Url = url,
                                IPAddress = fromIpAddress,
                                CreatedAt = now,
                                LastActivityAt = now,
                                UrlHash = url.ToUpperInvariant().Hash(),
                                User = (User) byUser,
                                Category = (Category) forCategory,
                            };
            return story;
        }

        public IStoryView CreateStoryView(IStory forStory, DateTime at, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInvalidDate(at, "at");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var view = new StoryView
                           {
                               Story = (Story) forStory,
                               IPAddress = fromIpAddress,
                               Timestamp = at,
                           };
            
            return view;
        }

        public IVote CreateStoryVote(IStory forStory, DateTime at, IUser byUser, string fromIpAddress)
        {
            Check.Argument.IsNotNull(forStory, "forStory");
            Check.Argument.IsNotInFuture(at, "at");
            Check.Argument.IsNotNull(byUser, "byUser");
            Check.Argument.IsNotEmpty(fromIpAddress, "fromIpAddress");

            var vote = new StoryVote
                           {
                               Story = (Story) forStory,
                               User = (User) byUser,
                               IPAddress = fromIpAddress,
                               Timestamp = at
                           };
            return vote;
        }
    }
}