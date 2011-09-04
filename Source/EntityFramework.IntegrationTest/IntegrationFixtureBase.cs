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

        protected override void DisposeCore()
        {
            if (dbFactory != null)
            {
                dbFactory.Dispose();
            }
        }
    }
}
