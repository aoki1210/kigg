﻿namespace Kigg.EF.Repository
{
    using System.Linq;
    using System.Data.Objects.DataClasses;

    using DomainObjects;

    public interface IDatabase
    {
        IQueryable<Category> CategoryDataSource
        {
            get;
        }

        IQueryable<Tag> TagDataSource
        {
            get;
        }

        IQueryable<Story> StoryDataSource
        {
            get;
        }

        IQueryable<StoryComment> CommentDataSource
        {
            get;
        }

        IQueryable<StoryVote> VoteDataSource
        {
            get;
        }

        IQueryable<StoryMarkAsSpam> MarkAsSpamDataSource
        {
            get;
        }

        IQueryable<StoryView> StoryViewDataSource
        {
            get;
        }

        IQueryable<User> UserDataSource
        {
            get;
        }

        IQueryable<UserScore> UserScoreDataSource
        {
            get;
        }

        IQueryable<KnownSource> KnownSourceDataSource
        {
            get;
        }

        IQueryable<TEntity> GetQueryable<TEntity>() 
            where TEntity : EntityObject;
        
        void InsertOnSubmit<TEntity>(TEntity entity)
            where TEntity : EntityObject;

        void DeleteOnSubmit<TEntity>(TEntity entity)
            where TEntity : EntityObject;

        void SubmitChanges();
    }
}
