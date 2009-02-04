﻿namespace Kigg.Repository.LinqToSql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DomainObjects;

    public class StoryRepository : BaseRepository<IStory, Story>, IStoryRepository
    {
        public StoryRepository(IDatabase database) : base(database)
        {
        }

        public StoryRepository(IDatabaseFactory factory) : base(factory)
        {
        }

        public override void Add(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Story story = (Story) entity;

            if (Database.StoryDataSource.Any(s => s.UrlHash == story.UrlHash))
            {
                throw new ArgumentException("\"{0}\" story with the same url already exits. Specifiy a diffrent url.".FormatWith(story.Url), "entity");
            }

            story.UniqueName = UniqueNameGenerator.GenerateFrom(Database.StoryDataSource, story.Title);

            base.Add(story);
        }

        public override void Remove(IStory entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Story story = (Story) entity;

            Database.DeleteAll(Database.StoryViewDataSource.Where(sv => sv.StoryId == story.Id));
            Database.DeleteAll(Database.CommentSubscribtionDataSource.Where(cs => cs.StoryId == story.Id));
            Database.DeleteAll(Database.CommentDataSource.Where(c => c.StoryId == story.Id));
            Database.DeleteAll(Database.VoteDataSource.Where(v => v.StoryId == story.Id));
            Database.DeleteAll(Database.MarkAsSpamDataSource.Where(sp => sp.StoryId == story.Id));
            Database.DeleteAll(Database.StoryTagDataSource.Where(st => st.StoryId == story.Id));

            base.Remove(story);
        }

        public virtual IStory FindById(Guid id)
        {
            Check.Argument.IsNotEmpty(id, "id");

            return Database.StoryDataSource.SingleOrDefault(s => s.Id == id);
        }

        public virtual IStory FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotEmpty(uniqueName, "uniqueName");

            return Database.StoryDataSource.SingleOrDefault(s => s.UniqueName == uniqueName);
        }

        public virtual IStory FindByUrl(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string hashedUrl = url.ToUpperInvariant().Hash();

            return Database.StoryDataSource.SingleOrDefault(s => s.UrlHash == hashedUrl);
        }

        public virtual PagedResult<IStory> FindPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByPublished();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPublishedByCategory(Guid categoryId, int start, int max)
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByCategory(categoryId);

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null) && (s.CategoryId == categoryId))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindUpcoming(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByUpcoming();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.PublishedAt == null) && (s.Rank == null))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindNew(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByNew();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.LastProcessedAt == null))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindUnapproved(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByUnapproved();

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt == null))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByPublishable(minimumDate, maximumDate);

            var stories = Database.StoryDataSource
                                  .Where(s => ((s.ApprovedAt != null) && ((s.CreatedAt >= minimumDate) && (s.CreatedAt <= maximumDate)) && ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt))))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start).Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindByTag(Guid tagId, int start, int max)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountByTag(tagId);

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && s.StoryTags.Any(st => st.TagId == tagId))
                                  .OrderByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> Search(string query, int start, int max)
        {
            Check.Argument.IsNotEmpty(query, "query");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => (s.ApprovedAt != null) && (s.Title.Contains(query) || s.UniqueName.Contains(query) || s.Url.Contains(query) || s.TextDescription.Contains(query) || s.Category.Name.Contains(query) || s.StoryTags.Any(st => st.Tag.Name.Contains(query)) || s.StoryComments.Any(c => c.TextBody.Contains(query))));

            var stories = Database.StoryDataSource
                                  .Where(s => (s.ApprovedAt != null) && (s.Title.Contains(query) || s.Url.Contains(query) || s.TextDescription.Contains(query) || s.Category.Name.Contains(query) || s.StoryTags.Any(st => st.Tag.Name.Contains(query)) || s.StoryComments.Any(c => c.TextBody.Contains(query))))
                                  .OrderByDescending(s => s.PublishedAt)
                                  .ThenBy(s => s.Rank)
                                  .ThenByDescending(s => s.CreatedAt)
                                  .Skip(start)
                                  .Take(max);

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPostedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = CountPostedByUser(userId);

            List<Story> stories = Database.StoryDataSource
                                          .Where(s => ((s.ApprovedAt != null) && (s.UserId == userId)))
                                          .OrderByDescending(s => s.CreatedAt)
                                          .Skip(start)
                                          .Take(max)
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindPromotedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => ((s.ApprovedAt != null) && s.StoryVotes.Any(v => v.UserId == userId)));

            List<Story> stories = Database.VoteDataSource
                                          .Where(v => ((v.UserId == userId) && (v.Story.ApprovedAt != null)))
                                          .OrderByDescending(v => v.Timestamp)
                                          .Select(v => v.Story)
                                          .Skip(start)
                                          .Take(max)
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual PagedResult<IStory> FindCommentedByUser(Guid userId, int start, int max)
        {
            Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            int total = Database.StoryDataSource
                                .Count(s => ((s.ApprovedAt != null) && s.StoryComments.Any(c => c.UserId == userId)));

            IQueryable<Guid> ids = Database.CommentDataSource
                                           .Where(c => ((c.UserId == userId) && (c.Story.ApprovedAt != null)))
                                           .OrderByDescending(c => c.CreatedAt)
                                           .Select(c => c.StoryId)
                                           .Distinct()
                                           .Skip(start)
                                           .Take(max);

            List<Story> stories = Database.StoryDataSource
                                          .Where(s => ids.Contains(s.Id))
                                          .ToList();

            return BuildPagedResult<IStory>(stories, total);
        }

        public virtual int CountByPublished()
        {
            return Database.StoryDataSource
                           .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null));
        }

        public virtual int CountByUpcoming()
        {
            return Database.StoryDataSource.Count(s => (s.ApprovedAt != null) && (s.PublishedAt == null));
        }

        public virtual int CountByCategory(Guid categoryId)
        {
            Check.Argument.IsNotEmpty(categoryId, "categoryId");

            return Database.StoryDataSource
                           .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.CategoryId == categoryId));
        }

        public virtual int CountByTag(Guid tagId)
        {
            Check.Argument.IsNotEmpty(tagId, "tagId");

            return Database.StoryDataSource.Count(s => (s.ApprovedAt != null) && s.StoryTags.Any(st => st.TagId == tagId));
        }

        public virtual int CountByNew()
        {
            return Database.StoryDataSource.Count(s => ((s.ApprovedAt != null) && (s.LastProcessedAt == null)));
        }

        public virtual int CountByUnapproved()
        {
            return Database.StoryDataSource.Count(s => s.ApprovedAt == null);
        }

        public virtual int CountByPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");

            return Database.StoryDataSource.Count(s => ((s.ApprovedAt != null) && ((s.CreatedAt >= minimumDate) && (s.CreatedAt <= maximumDate)) && ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt))));
        }

        public virtual int CountPostedByUser(Guid userId)
        {
            Check.Argument.IsNotEmpty(userId, "userId");

            return Database.StoryDataSource.Count(s => (s.ApprovedAt != null) && (s.UserId == userId));
        }
    }
}