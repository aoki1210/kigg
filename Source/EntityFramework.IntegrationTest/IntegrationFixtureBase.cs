namespace Kigg.Infrastructure.EntityFramework.IntegrationTest
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using System.Data.Common;

    using FizzWare.NBuilder;

    using Domain.Entities;

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
            var category = new Category { Name = "C#", UniqueName = "C-Sharp", CreatedAt = SystemTime.Now };

            if (persist)
            {
                Context.Categories.Add(category);

                Context.SaveChanges();
            }

            return category;
        }
        protected Tag NewTag(bool persist)
        {
            var tag = new Tag { Name = "C#", UniqueName = "C-Sharp", CreatedAt = SystemTime.Now };

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
                               Role = Role.User,
                               CreatedAt = SystemTime.Now,
                               LastActivityAt = SystemTime.Now,
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

        protected IList<TDomainObject> NewDomainObjectList<TDomainObject>(bool persist = true)
            where TDomainObject : class, IDomainObject
        {
            return NewDomainObjectList<TDomainObject>(10, persist);
        }

        protected IList<TDomainObject> NewDomainObjectList<TDomainObject>(int size, bool persist = true)
            where TDomainObject : class, IDomainObject
        {
            BuilderSetup.SetCreatePersistenceMethod<IList<TDomainObject>>(list =>
            {
                list.ForEach(
                    o => Context.Set<TDomainObject>().Add(o));
                Context.SaveChanges();
            });

            IListBuilder<TDomainObject> builder = Builder<TDomainObject>.CreateListOfSize(size);

            var objects = persist ? builder.Persist() : builder.Build();

            return objects;
        }

        protected IList<TDomainObject> NewDomainObjectList<TDomainObject>(int size, Action<TDomainObject> initializer, bool persist = true)
            where TDomainObject : class, IDomainObject
        {
            BuilderSetup.SetCreatePersistenceMethod<IList<TDomainObject>>(list =>
                                                                              {
                                                                                  list.ForEach(
                                                                                      o =>
                                                                                      Context.Set<TDomainObject>().Add(o));
                                                                                  Context.SaveChanges();
                                                                              });

            var builder = Builder<TDomainObject>.CreateListOfSize(10)
                                                .All()
                                                .Do(initializer);

            var objects = persist ? builder.Persist() : builder.Build();

            return objects;
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
