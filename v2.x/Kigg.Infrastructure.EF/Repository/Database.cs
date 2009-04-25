namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Data.Objects;
    using System.Data.Metadata.Edm;
    using System.Data.EntityClient;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;

    using DomainObjects;

    public class Database : ObjectContext, IDatabase
    {
        internal const string _defaultContainerName = "KiggEntityContainer";
        
        private readonly static IDictionary<string,string> EntitySetNames = new Dictionary<string, string>(10);

        internal DataLoadOptions LoadOptions { get; set; }

        public Database(string connectionString) :
            base(connectionString, _defaultContainerName)
        {   
        }

        public Database(EntityConnection connection) :
            base(connection, _defaultContainerName)
        {
        }

        public string GetEntitySetName(Type entitySetType)
        {
            lock (EntitySetNames)
            {
                if (EntitySetNames.ContainsKey(entitySetType.FullName))
                {
                    return EntitySetNames[entitySetType.FullName];
                }

                var container = MetadataWorkspace.GetEntityContainer(DefaultContainerName, DataSpace.CSpace);

                var entitySetName = (from meta in container.BaseEntitySets
                                     where meta.BuiltInTypeKind == BuiltInTypeKind.EntitySet &&
                                     meta.ElementType.Name == entitySetType.Name
                                     select meta.Name).First();

                EntitySetNames.Add(entitySetType.FullName, entitySetName);

                return entitySetName;
            }
        }

        public IQueryable<Category> CategoryDataSource
        {
            get
            {
                return GetQueryable<Category>();
            }
        }

        public IQueryable<Tag> TagDataSource
        {
            get
            {
                return GetQueryable<Tag>();
            }
        }

        public IQueryable<Story> StoryDataSource
        {
            get
            {
                return GetQueryable<Story>();
            }
        }

        public IQueryable<StoryComment> CommentDataSource
        {
            get
            {
                return GetQueryable<StoryComment>();
            }
        }

        public IQueryable<StoryVote> VoteDataSource
        {
            get
            {
                return GetQueryable<StoryVote>();
            }
        }

        public IQueryable<StoryMarkAsSpam> MarkAsSpamDataSource
        {
            get
            {
                return GetQueryable<StoryMarkAsSpam>();
            }
        }

        public IQueryable<StoryView> StoryViewDataSource
        {
            get
            {
                return GetQueryable<StoryView>();
            }
        }

        public IQueryable<User> UserDataSource
        {
            get
            {
                return GetQueryable<User>();
            }
        }

        public IQueryable<UserScore> UserScoreDataSource
        {
            get
            {
                return GetQueryable<UserScore>();
            }
        }

        public IQueryable<KnownSource> KnownSourceDataSource
        {
            get
            {
                return GetQueryable<KnownSource>();
            }
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : EntityObject
        {
            var entitySetName = GetEntitySetName(typeof(TEntity));
            return GetQueryable<TEntity>(entitySetName);

        }
        
        public IQueryable<TEntity> GetQueryable<TEntity>(string queryString) where TEntity : EntityObject
        {
            return ApplyDataLoadOptions<TEntity>(queryString);
        }

        public void InsertOnSubmit<TEntity>(TEntity entity) where TEntity : EntityObject
        {
            var entitySetName = GetEntitySetName(typeof(TEntity));
            AddObject(entitySetName, entity);
        }

        public void DeleteOnSubmit<TEntity>(TEntity entity) where TEntity : EntityObject
        {
            DeleteObject(entity);
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }

        private ObjectQuery<TEntity> ApplyDataLoadOptions<TEntity>(string queryString)
        {
            var query = CreateQuery<TEntity>(queryString);

            if (LoadOptions != null)
            {
                var members = LoadOptions.GetPreloadedMembers<TEntity>();

                foreach (var member in members)
                {
                    query = query.Include(member.Name);
                }
            }
            return query;
        }

    }
}
