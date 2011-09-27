namespace Kigg.Infrastructure.EntityFramework
{
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using DomainObjects;

    public class KiggDbContext : DbContext
    {
        public KiggDbContext(string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {
        }

        public KiggDbContext(DbConnection existingConnection, DbCompiledModel model)
            : base(existingConnection, model, true)
        {
        }

        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Story> Stories { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<UserScore> Scores { get; set; }
        public IDbSet<Tag> Tags { get; set; }
        public IDbSet<KnownSource> KnownSources { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        internal IDbSet<Vote> Votes { get; set; }
        internal IDbSet<StoryView> Views { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity: class
        {
            return base.Set<TEntity>();
        }
    }
}
