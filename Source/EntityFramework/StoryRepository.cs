namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Linq;
   
    using Domain.Entities;
    using Query;
    using Repository;
    

    public class StoryRepository : EntityRepositoryBase<Story>, IStoryRepository
    {
        public StoryRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public override void Add(Story story)
        {
            Check.Argument.IsNotNull(story, "story");


            if (DbContext.Stories.Any(s => s.UrlHash == story.UrlHash))
            {
                throw new ArgumentException("\"{0}\" story with the same url already exits. Specifiy a diffrent url.".FormatWith(story.Url), "story");
            }

            story.UniqueName = UniqueNameGenerator.GenerateFrom(DbContext.Stories, story.Title);

            base.Add(story);
        }

        public override void Remove(Story story)
        {
            Check.Argument.IsNotNull(story, "story");

            DbContext.Remove<Tag>(DbContext.Tags.Where(t => t.Stories.Any(s=>s.Id == story.Id)));
            DbContext.Remove<StoryView>(DbContext.Views.Where(v => v.ForStory.Id == story.Id));
            DbContext.Remove<Vote>(DbContext.Votes.Where(v => v.StoryId == story.Id));
            DbContext.Remove<SpamVote>(DbContext.Spams.Where(s => s.StoryId == story.Id));
            //entity.RemoveAllCommentSubscribers();
            DbContext.Remove<Comment>(DbContext.Comments.Where(c => c.ForStory.Id == story.Id));
            
            base.Remove(story);
        }

        public Story FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotNullOrEmpty(uniqueName, "uniqueName");

            var query = QueryFactory.CreateFindStoryByUniqueName(uniqueName);
            
            return query.Execute();
        }

        public Story FindByUrl(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            string hashedUrl = url.ToUpperInvariant().Hash();

            var query = QueryFactory.CreateFindStoryByUrl(hashedUrl);

            return query.Execute();
        }

        public PagedResult<Story> FindPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var query = QueryFactory.CreateFindPublishedStories(start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindPublishedByCategory(long categoryId, int start, int max)
        {
            Check.Argument.IsNotZeroOrNegative(categoryId, "categoryId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindPublishedStoriesByCategory(categoryId, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindPublishedByCategory(string category, int start, int max)
        {
            Check.Argument.IsNotNullOrEmpty(category, "category");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindPublishedStoriesByCategory(category, start, max);

            return CreatePagedResult(query);
        }
        
        public PagedResult<Story> FindUpcoming(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");
            
            var query = QueryFactory.CreateFindUpcomingStories(start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindNew(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindNewStories(start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindUnapproved(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindUnapprovedStories(start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindPublishableStories(minimumDate, maximumDate, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindByTag(long tagId, int start, int max)
        {
            Check.Argument.IsNotZeroOrNegative(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindStoriesByTag(tagId, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindByTag(string tagName, int start, int max)
        {
            Check.Argument.IsNotNullOrEmpty(tagName, "tagName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindStoriesByTag(tagName, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindPostedByUser(long userId, int start, int max)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindPostedStoriesByUser(userId, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindPostedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotNullOrEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindPostedStoriesByUser(userName, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindPromotedByUser(long userId, int start, int max)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindPromotedStoriesByUser(userId, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindPromotedByUser(string userName, int start, int max)
        {
            Check.Argument.IsNotNullOrEmpty(userName, "userName");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindPromotedStoriesByUser(userName, start, max);

            return CreatePagedResult(query);
        }

        public PagedResult<Story> FindCommentedByUser(long userId, int start, int max)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotZeroOrNegative(max, "max");

            var query = QueryFactory.CreateFindCommentedStoriesByUser(userId, start, max);

            return CreatePagedResult(query);
        }
        
        public int CountPublished()
        {
            var query = QueryFactory.CreateCountPublishedStories();

            return query.Execute();
        }

        public int CountUpcoming()
        {
            var query = QueryFactory.CreateCountUpcomingStories();

            return query.Execute();
        }

        public int CountByCategory(long categoryId)
        {
            Check.Argument.IsNotZeroOrNegative(categoryId, "categoryId");

            var query = QueryFactory.CreateCountPublishedStoriesByCategory(categoryId);

            return query.Execute();
        }

        public int CountByCategory(string categoryName)
        {
            Check.Argument.IsNotNullOrEmpty(categoryName, "categoryName");

            var query = QueryFactory.CreateCountPublishedStoriesByCategory(categoryName);

            return query.Execute();
        }

        public int CountByTag(long tagId)
        {
            Check.Argument.IsNotZeroOrNegative(tagId, "tagId");

            var query = QueryFactory.CreateCountStoriesByTag(tagId);

            return query.Execute();
        }

        public int CountByTag(string tagName)
        {
            Check.Argument.IsNotNullOrEmpty(tagName, "tagName");

            var query = QueryFactory.CreateCountStoriesByTag(tagName);

            return query.Execute();
        }

        public int CountNew()
        {
            var query = QueryFactory.CreateCountNewStories();

            return query.Execute(); 
        }

        public int CountUnapproved()
        {
            var query = QueryFactory.CreateCountUnapprovedStories();

            return query.Execute(); 
        }

        public int CountPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            
            var query = QueryFactory.CreateCountPublishableStories(minimumDate, maximumDate);

            return query.Execute();
        }

        public int CountPostedByUser(long userId)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");

            var query = QueryFactory.CreateCountPostedStoriesByUser(userId);

            return query.Execute(); 
        }

        public int CountPostedByUser(string userName)
        {
            Check.Argument.IsNotNullOrEmpty(userName, "userName");

            var query = QueryFactory.CreateCountPostedStoriesByUser(userName);

            return query.Execute();
        }

    }
}