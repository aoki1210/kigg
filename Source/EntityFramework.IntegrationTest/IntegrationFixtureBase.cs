namespace Kigg.Infrastructure.EntityFramework.IntegrationTest
{
    using System.Data.Common;
    using System.Configuration;
    using System.Collections.Generic;

    using DomainObjects;

    public abstract class IntegrationFixtureBase : Disposable
    {
        protected readonly KiggDbFactory dbFactory;
        protected KiggDbContext Context
        {
            get { return dbFactory.Get(); }
        }

        protected IntegrationFixtureBase()
        {
            var connConfig = ConfigurationManager.ConnectionStrings["KiGG"];
            DbProviderFactory provider = DbProviderFactories.GetFactory(connConfig.ProviderName);
            var connString = connConfig.ConnectionString;
            dbFactory = new KiggDbFactory(provider, connString);
        }

        protected Category NewCategory(bool persist)
        {
            var category = new Category { Name = "C#", UniqueName = "C-Sharp", CreatedAt = SystemTime.Now() };

            if (persist)
            {
                Context.Categories.Add(category);

                Context.SaveChanges();
            }

            return category;
        }
        protected Tag NewTag(bool persist)
        {
            var tag = new Tag { Name = "C#", UniqueName = "C-Sharp", CreatedAt = SystemTime.Now()};

            if (persist)
            {
                Context.Tags.Add(tag);

                Context.SaveChanges();
            }

            return tag;
        }
        protected User NewUser(bool persist)
        {
            var user = new User
                           {
                               UserName = "mosessaur",
                               Email = "mosessaur@twitter.com",
                               Role = Roles.User,
                               CreatedAt = SystemTime.Now(),
                               LastActivityAt = SystemTime.Now(),
                               IsLockedOut = false,
                               IsActive = true,
                           };

            if (persist)
            {
                Context.Users.Add(user);

                Context.SaveChanges();
            }

            return user;
        }

        protected IList<Category> NewCategoryList(bool persist)
        {
            var categories = new List<Category>(11);
            for (int i = 1; i < 11; i++)
            {
                var category = new Category
                                   {
                                       Name = "Category {0}".FormatWith(i),
                                       UniqueName = "Category-{0}".FormatWith(i),
                                       CreatedAt = SystemTime.Now()
                                   };

                categories.Add(category);
            }

            if (persist)
            {
                categories.ForEach(c => Context.Categories.Add(c));
                Context.SaveChanges();
            }

            return categories;
        }
        protected IList<Tag> NewTagList(bool persist)
        {
            var tags = new List<Tag>(11);
            for (int i = 1; i < 11; i++)
            {
                var tag = new Tag
                {
                    Name = "Tag {0}".FormatWith(i),
                    UniqueName = "Tag-{0}".FormatWith(i),
                    CreatedAt = SystemTime.Now()
                };

                tags.Add(tag);
            }

            if (persist)
            {
                tags.ForEach(t => Context.Tags.Add(t));
                Context.SaveChanges();
            }

            return tags;
        }
        protected IList<User> NewUserList(bool persist)
        {
            var users = new List<User>(11);
            for (int i = 1; i < 11; i++)
            {
                var user = new User
                {
                    UserName = "user{0}".FormatWith(i),
                    Email = "user{0}@twitter.com".FormatWith(i),
                    Role = Roles.User,
                    CreatedAt = SystemTime.Now(),
                    LastActivityAt = SystemTime.Now(),
                    IsLockedOut = false,
                    IsActive = true,
                };

                users.Add(user);
            }

            if (persist)
            {
                users.ForEach(c => Context.Users.Add(c));
                Context.SaveChanges();
            }

            return users;
        }
        protected IList<UserScore> GenerateScoreForUser(User user, int score, int count, bool persist)
        {
            var scores = new List<UserScore>();
            for (int i = 0; i < count; i++)
            {
                var userScore = new UserScore
                                {
                                    ScoredBy = user,
                                    Action = UserAction.StorySubmitted,
                                    Score = score,
                                    CreatedAt = SystemTime.Now().AddDays(-i)
                                };
                scores.Add(userScore);
            }
            if (persist)
            {
                scores.ForEach(us => Context.Scores.Add(us));
                Context.SaveChanges();
            }
            return scores;
        }

        protected override void DisposeCore()
        {
            if (dbFactory != null)
            {
                dbFactory.Dispose();
            }
        }
    }
}
