namespace Kigg.Infrastructure.EntityFramework.Query
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;

    using Domain.Entities;

    public class QueryFactory : IQueryFactory
    {
        private KiggDbContext dbContext;

        private KiggDbContext DbContext
        {
            [DebuggerStepThrough]
            get
            {
                return dbContext ?? (dbContext = dbContextFactory.Get());
            }
        }

        public bool UseCompiled { get; private set; }

        private readonly IKiggDbFactory dbContextFactory;

        public QueryFactory(IKiggDbFactory dbContextFactory, bool useCompiled)
        {
            Check.Argument.IsNotNull(dbContextFactory, "dbContextFactory");
            UseCompiled = useCompiled;
            this.dbContextFactory = dbContextFactory;
        }

        public IQuery<int> CreateCountVotesByStory(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            var query = new CountVotesQuery(DbContext, v => v.StoryId == id);

            return query;
        }

        public IQuery<int> CreateCountSpamVotesByStory(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            var query = new CountSpamVotesQuery(DbContext, s => s.StoryId == id);

            return query;
        }

        public IQuery<int> CreateCountViewsByStory(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            var query = new CountStoryViewsQuery(DbContext, v => v.ForStory.Id == id);

            return query;
        }

        public IQuery<int> CreateCountCommentsByStory(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            var query = new CountCommentsQuery(DbContext, c => c.ForStory.Id == id);

            return query;
        }

        public IQuery<int> CreateCountNewStories()
        {
            var query = new CountStoriesQuery(DbContext, s => s.ApprovedAt != null &&
                                                              s.LastProcessedAt == null);

            return query;
        }

        public IQuery<int> CreateCountUpcomingStories()
        {
            var query = new CountStoriesQuery(DbContext, s => s.ApprovedAt != null &&
                                                              s.PublishedAt == null &&
                                                              s.Rank == null);

            return query;
        }

        public IQuery<int> CreateCountUnapprovedStories()
        {
            var query = new CountStoriesQuery(DbContext, s => s.ApprovedAt == null);

            return query;
        }

        public IQuery<int> CreateCountPublishableStories(DateTime minimumDate, DateTime maximumDate)
        {
            var query = new CountStoriesQuery(DbContext, s => (s.ApprovedAt >= minimumDate &&
                                                               s.ApprovedAt <= maximumDate) &&
                                                              (s.LastProcessedAt == null ||
                                                               s.LastProcessedAt <= s.LastActivityAt));

            return query;
        }

        public IQuery<int> CreateCountPublishedStories()
        {
            var query = new CountStoriesQuery(DbContext, s => s.ApprovedAt != null && s.PublishedAt != null);

            return query;
        }

        public IQuery<int> CreateCountPublishedStoriesByCategory(long categoryId)
        {
            var query = new CountStoriesQuery(DbContext,
                                              s => s.ApprovedAt != null &&
                                                   s.PublishedAt != null &&
                                                   s.BelongsTo.Id == categoryId);

            return query;
        }

        public IQuery<int> CreateCountPublishedStoriesByCategory(string category)
        {
            var query = new CountStoriesQuery(DbContext,
                                              s => s.ApprovedAt != null &&
                                                   s.PublishedAt != null &&
                                                   s.BelongsTo.Name == category);

            return query;
        }

        public IQuery<int> CreateCountStoriesByTag(string tag)
        {
            var query = new CountStoriesQuery(DbContext,
                                              s => s.ApprovedAt != null &&
                                                   s.Tags.Any(t => t.Name == tag));

            return query;
        }

        public IQuery<int> CreateCountStoriesByTag(long tagId)
        {
            var query = new CountStoriesQuery(DbContext,
                                              s => s.ApprovedAt != null &&
                                                   s.Tags.Any(t => t.Id == tagId));

            return query;
        }

        public IQuery<int> CreateCountPostedStoriesByUser(long userId)
        {
            var query = new CountStoriesQuery(DbContext,
                                              s => s.ApprovedAt != null &&
                                                   s.PostedBy.Id == userId);

            return query;
        }

        public IQuery<int> CreateCountPostedStoriesByUser(string userName)
        {
            var query = new CountStoriesQuery(DbContext,
                                              s => s.ApprovedAt != null &&
                                                   s.PostedBy.UserName == userName);

            return query;
        }

        public IQuery<decimal> CreateCalculateUserScoreById(long id, DateTime startDate, DateTime endDate)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");
            Check.Argument.IsNotInvalidDate(startDate, "startDate");
            Check.Argument.IsNotInFuture(startDate, "startDate");
            Check.Argument.IsNotInvalidDate(endDate, "endDate");
            Check.Argument.IsNotInFuture(endDate, "endDate");

            var query = new CalculateUserScoreQuery(DbContext,
                                                        us =>
                                                        (us.ScoredBy.Id == id) &&
                                                        (us.CreatedAt >= startDate && us.CreatedAt <= endDate));

            return query;
        }

        public IQuery<Category> CreateFindUniqueCategoryByUniqueName(string uniqueName)
        {
            var query = CreateFindUniqueDomainObjectQuery<Category>(c => c.UniqueName == uniqueName);

            return query;
        }

        public IQuery<Category> CreateFindUniqueCategoryByName(string name)
        {
            var query = CreateFindUniqueDomainObjectQuery<Category>(c => c.Name == name);

            return query;
        }

        public IOrderedQuery<Category> CreateFindAllCategories<TKey>(Expression<Func<Category, TKey>> orderBy)
        {
            IOrderedQuery<Category> query = CreateFindDomainObjectListQuery<Category>();

            query = query.OrderBy(orderBy);

            return query;
        }

        public IQuery<KnownSource> CreateFindKnownSourceByUrl(string url)
        {
            var query = CreateFindUniqueDomainObjectQuery<KnownSource>(k => k.Url == url);
            return query;
        }

        public IOrderedQuery<KnownSource> CreateFindAllKnownSources<TKey>(Expression<Func<KnownSource, TKey>> orderBy)
        {
            IOrderedQuery<KnownSource> query = CreateFindDomainObjectListQuery<KnownSource>();

            query = query.OrderBy(orderBy);

            return query;
        }

        public IQuery<Tag> CreateFindUniqueTagByUniqueName(string uniqueName)
        {
            var query = CreateFindUniqueDomainObjectQuery<Tag>(t => t.UniqueName == uniqueName);

            return query;
        }

        public IQuery<Tag> CreateFindUniqueTagByName(string name)
        {
            var query = CreateFindUniqueDomainObjectQuery<Tag>(t => t.Name == name);

            return query;
        }

        public IOrderedQuery<Tag> CreateFindTagsByMatchingName(string name, int max)
        {
            IOrderedQuery<Tag> query = CreateFindDomainObjectListQuery<Tag>(t => t.Name.StartsWith(name));

            query = query.OrderBy(t => t.Name).Limit(max);

            return query;
        }

        public IOrderedQuery<Tag> CreateFindTagsByUsage(int max)
        {
            IOrderedQuery<Tag> query = CreateFindDomainObjectListQuery<Tag>(t => t.Stories.Any());

            query = query.OrderByDescending(t => t.Stories.Count(st => st.ApprovedAt != null))
                         .ThenBy(t => t.Name)
                         .Limit(max);

            return query;
        }

        public IOrderedQuery<Tag> CreateFindAllTags<TKey>(Expression<Func<Tag, TKey>> orderBy)
        {
            IOrderedQuery<Tag> query = CreateFindDomainObjectListQuery<Tag>();

            query = query.OrderBy(orderBy);

            return query;
        }
        
        public IQuery<User> CreateFindUserById(long id)
        {
            var query = CreateFindUniqueDomainObjectQuery<User>(u => u.Id == id);

            return query;
        }

        public IQuery<User> CreateFindUserByEmail(string email)
        {
            var query = CreateFindUniqueDomainObjectQuery<User>(u => u.Email == email);

            return query;
        }

        public IQuery<User> CreateFindUserByUserName(string userName)
        {
            var query = CreateFindUniqueDomainObjectQuery<User>(u => u.UserName == userName);

            return query;
        }

        public IOrderedQuery<User> CreateFindTopScoredUsers(DateTime startDate, DateTime endDate, int start, int max)
        {
            Check.Argument.IsNotInvalidDate(startDate, "startDate");
            Check.Argument.IsNotInFuture(startDate, "startDate");
            Check.Argument.IsNotInvalidDate(endDate, "endDate");
            Check.Argument.IsNotInFuture(endDate, "endDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var query = new UserFindTopScoredListQuery(DbContext, us => (us.ScoredBy.Role == Role.User) && (!us.ScoredBy.IsLockedOut) && (us.CreatedAt >= startDate && us.CreatedAt <= endDate));
            return query.Page(start, max);
        }

        public IOrderedQuery<User> CreateFindAllUsers<TKey>(int start, int max, Expression<Func<User, TKey>> orderBy)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var query = CreateFindDomainObjectListQuery<User>();

            return query.OrderBy(orderBy).ThenByDescending(u => u.LastActivityAt).Page(start, max);
        }

        public IOrderedQuery<Vote> CreateFindVotesAfterDate(long storyId, DateTime date)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotInFuture(date, "date");
            Check.Argument.IsNotInvalidDate(date, "date");

            var query = CreateFindDomainObjectListQuery<Vote>(v => v.StoryId == storyId && v.PromotedAt >= date);

            return query;
        }

        public IQuery<Vote> CreateFindVoteById(long userId, long storyId)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");

            var query = CreateFindUniqueDomainObjectQuery<Vote>(v => v.UserId == userId && v.StoryId == storyId);

            return query;
        }

        public IQuery<SpamVote> CreateFindSpamVoteById(long userId, long storyId)
        {
            Check.Argument.IsNotZeroOrNegative(userId, "userId");
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");

            var query = CreateFindUniqueDomainObjectQuery<SpamVote>(s => s.UserId == userId && s.StoryId == storyId);

            return query;
        }

        public IOrderedQuery<SpamVote> CreateFindSpamVotesAfterDate(long storyId, DateTime date)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotInFuture(date, "date");
            Check.Argument.IsNotInvalidDate(date, "date");

            var query = CreateFindDomainObjectListQuery<SpamVote>(v => v.StoryId == storyId && v.MarkedAt >= date);

            return query;
        }

        public IOrderedQuery<StoryView> CreateFindStoryViewsAfterDate(long storyId, DateTime date)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotInFuture(date, "date");
            Check.Argument.IsNotInvalidDate(date, "date");

            var query = CreateFindDomainObjectListQuery<StoryView>(v => v.ForStory.Id == storyId && v.ViewedAt >= date);

            return query;
        }

        public IOrderedQuery<Comment> CreateFindCommentsForStoryAfterDate(long storyId, DateTime date, int? start = null, int? max = null)
        {
            Check.Argument.IsNotZeroOrNegative(storyId, "storyId");
            Check.Argument.IsNotInvalidDate(date, "date");

            IOrderedQuery<Comment> query = CreateFindDomainObjectListQuery<Comment>(c => c.ForStory.Id == storyId && c.CreatedAt >= date);

            query = query.OrderBy(c => c.CreatedAt);

            if (start != null && max != null)
            {
                query = query.Page(start.Value, max.Value);
            }

            return query;
        }

        public IQuery<Comment> CreateFindCommentById(long id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            var query = CreateFindUniqueDomainObjectQuery<Comment>(c => c.Id == id);

            return query;
        }

        public IQuery<Story> CreateFindStoryById(long id)
        {
            var query = CreateFindUniqueDomainObjectQuery<Story>(u => u.Id == id);

            return query;
        }

        public IQuery<Story> CreateFindStoryByUniqueName(string uniqueName)
        {
            var query = CreateFindUniqueDomainObjectQuery<Story>(u => u.UniqueName == uniqueName);

            return query;
        }

        public IQuery<Story> CreateFindStoryByUrl(string urlHash)
        {
            var query = CreateFindUniqueDomainObjectQuery<Story>(u => u.UrlHash == urlHash);

            return query;
        }

        public IOrderedQuery<Story> CreateFindPublishedStories(int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(
                                                                              s =>
                                                                              s.ApprovedAt != null &&
                                                                              s.PublishedAt != null &&
                                                                              s.Rank != null);

            return query.OrderByDescending(s => s.PublishedAt)
                        .ThenBy(s => s.Rank)
                        .ThenByDescending(s => s.CreatedAt)
                        .Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindPublishedStoriesByCategory(long categoryId, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(
                                                                              s =>
                                                                              s.ApprovedAt != null &&
                                                                              s.PublishedAt != null &&
                                                                              s.Rank != null &&
                                                                              s.BelongsTo.Id == categoryId);

            return query.OrderByDescending(s => s.PublishedAt)
                        .ThenBy(s => s.Rank)
                        .ThenByDescending(s => s.CreatedAt)
                        .Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindPublishedStoriesByCategory(string category, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(
                                                                              s =>
                                                                              s.ApprovedAt != null &&
                                                                              s.PublishedAt != null &&
                                                                              s.Rank != null &&
                                                                              s.BelongsTo.Name == category);

            return query.OrderByDescending(s => s.PublishedAt)
                        .ThenBy(s => s.Rank)
                        .ThenByDescending(s => s.CreatedAt)
                        .Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindUpcomingStories(int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(
                                                                              s =>
                                                                              s.ApprovedAt != null &&
                                                                              s.PublishedAt == null &&
                                                                              s.Rank == null);

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindStoriesByTag(long tagId, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(s => s.ApprovedAt != null &&
                                                                                     s.Tags.Any(t => t.Id == tagId));

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindStoriesByTag(string tag, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(s => s.ApprovedAt != null &&
                                                                                     s.Tags.Any(t => t.Name == tag));

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindPostedStoriesByUser(long userId, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(s => s.ApprovedAt != null &&
                                                                                     s.PostedBy.Id == userId);

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindPostedStoriesByUser(string userName, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(s => s.ApprovedAt != null &&
                                                                                     s.PostedBy.UserName == userName);

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindPromotedStoriesByUser(long userId, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(s => s.ApprovedAt != null &&
                                                                                     s.Votes.Any(v => v.UserId == userId));

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindPromotedStoriesByUser(string userName, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(s => s.ApprovedAt != null &&
                                                                                     s.Votes.Any(v => v.ByUser.UserName == userName));

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindCommentedStoriesByUser(long userId, int start, int max)
        {
            IOrderedQuery<Story> query =
                CreateFindDomainObjectListQuery<Story>(s => s.Comments.Any(c => c.ByUser.Id == userId));

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindNewStories(int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(
                                                                              s =>
                                                                              s.ApprovedAt != null &&
                                                                              s.LastProcessedAt == null);

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindUnapprovedStories(int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(
                                                                              s =>
                                                                              s.ApprovedAt == null);

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        public IOrderedQuery<Story> CreateFindPublishableStories(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            IOrderedQuery<Story> query = CreateFindDomainObjectListQuery<Story>(s => (s.ApprovedAt >= minimumDate &&
                                                                                      s.ApprovedAt <= maximumDate) &&
                                                                                     (s.LastProcessedAt == null ||
                                                                                      s.LastProcessedAt <=
                                                                                      s.LastActivityAt));

            return query.OrderByDescending(s => s.CreatedAt).Page(start, max);
        }

        private DomainObjectFindUniqueQuery<TResult> CreateFindUniqueDomainObjectQuery<TResult>(Expression<Func<TResult, bool>> predicate)
            where TResult : class, IDomainObject
        {
            return new DomainObjectFindUniqueQuery<TResult>(DbContext, predicate);
        }

        private DomainObjectFindListQuery<TResult> CreateFindDomainObjectListQuery<TResult>()
            where TResult : class, IDomainObject
        {
            return CreateFindDomainObjectListQuery<TResult>(null);
        }

        private DomainObjectFindListQuery<TResult> CreateFindDomainObjectListQuery<TResult>(Expression<Func<TResult, bool>> predicate)
            where TResult : class, IDomainObject
        {

            return (predicate != null)
                       ? new DomainObjectFindListQuery<TResult>(DbContext, predicate)
                       : new DomainObjectFindListQuery<TResult>(DbContext);
        }
    }
}
