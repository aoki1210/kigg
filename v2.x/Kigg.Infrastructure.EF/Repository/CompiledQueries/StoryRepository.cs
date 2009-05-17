namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Data.Objects;

    using DomainObjects;

    public partial class StoryRepository
    {
        private static readonly Func<Database, Guid, Story>
            FindByIdQuery = CompiledQuery.Compile<Database, Guid, Story>(
                (db, id) => db.StorySet.FirstOrDefault(s => s.Id == id));

        private static readonly Func<Database, string, Story>
            FindByUniqueNameQuery = CompiledQuery.Compile<Database, string, Story>(
                (db, uniqueName) => db.StorySet.FirstOrDefault(s => s.UniqueName == uniqueName));

        private static readonly Func<Database, string, Story>
            FindByUrlQuery = CompiledQuery.Compile<Database, string, Story>(
                (db, urlHash) => db.StorySet.FirstOrDefault(s => s.UrlHash == urlHash));

        private static readonly Func<Database, int, int, IQueryable<Story>>
            FindPublishedQuery = CompiledQuery.Compile<Database, int, int, IQueryable<Story>>(
                (db, start, max) => db.StorySet
                                      .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null))
                                      .OrderByDescending(s => s.PublishedAt)
                                      .ThenBy(s => s.Rank)
                                      .ThenByDescending(s => s.CreatedAt)
                                      .Skip(start).Take(max));

        private static readonly Func<Database, Guid, int, int, IQueryable<Story>>
            FindPublishedByCategoryQuery = CompiledQuery.Compile<Database, Guid, int, int, IQueryable<Story>>(
                (db, id, start, max) => db.StorySet
                                          .Where(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Rank != null) && (s.Category.Id == id))
                                          .OrderByDescending(s => s.PublishedAt)
                                          .ThenBy(s => s.Rank)
                                          .ThenByDescending(s => s.CreatedAt)
                                          .Skip(start).Take(max));

        private static readonly Func<Database, int, int, IQueryable<Story>>
            FindUpcomingQuery = CompiledQuery.Compile<Database, int, int, IQueryable<Story>>(
                (db, start, max) => db.StorySet
                                      .Where(s => (s.ApprovedAt != null) && (s.PublishedAt == null) && (s.Rank == null))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start).Take(max));

        private static readonly Func<Database, int, int, IQueryable<Story>>
            FindNewQuery = CompiledQuery.Compile<Database, int, int, IQueryable<Story>>(
                (db, start, max) => db.StorySet
                                      .Where(s => (s.ApprovedAt != null) && (s.LastProcessedAt == null))
                                      .OrderByDescending(s => s.CreatedAt)
                                      .Skip(start).Take(max));

        private static readonly Func<Database, FindPublishableQueryParameter, IQueryable<Story>>
            FindPublishableQuery = CompiledQuery.Compile<Database, FindPublishableQueryParameter, IQueryable<Story>>(
                (db, param) => db.StorySet
                                 .Where(s => (((s.ApprovedAt >= param.minimumDate) && (s.ApprovedAt <= param.maximumDate)) && ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt))))
                                 .OrderByDescending(s => s.CreatedAt)
                                 .Skip(param.start).Take(param.max));

        private static readonly Func<Database, Guid, int, int, IQueryable<Story>>
            FindByTagQuery = CompiledQuery.Compile<Database, Guid, int, int, IQueryable<Story>>(
                (db, id, start, max) => db.StorySet
                                          .Where(s => (s.ApprovedAt != null) && s.StoryTagsInternal.Any(t => t.Id == id))
                                          .OrderByDescending(s => s.CreatedAt)
                                          .Skip(start).Take(max));

        private static readonly Func<Database, Guid, int, int, IQueryable<Story>>
            FindPostedByUserQuery = CompiledQuery.Compile<Database, Guid, int, int, IQueryable<Story>>(
                (db, id, start, max) => db.StorySet
                                          .Where(s => ((s.ApprovedAt != null) && (s.User.Id == id)))
                                          .OrderByDescending(s => s.CreatedAt)
                                          .Skip(start).Take(max));

        private static readonly Func<Database, Guid, int, int, IQueryable<Story>>
            FindPromotedByUserQuery = CompiledQuery.Compile<Database, Guid, int, int, IQueryable<Story>>(
                (db, id, start, max) => db.VoteSet
                                          .Where(v => ((v.User.Id == id) && (v.Story.ApprovedAt != null)))
                                          .OrderByDescending(v => v.Timestamp)
                                          .Select(v => v.Story)
                                          .Skip(start).Take(max));

        private static readonly Func<Database, Guid, int, int, IQueryable<Story>>
            FindCommentedByUserQuery = CompiledQuery.Compile<Database, Guid, int, int, IQueryable<Story>>(
                (db, id, start, max) => db.CommentSet
                                          .Where(c1 => db.StorySet.Where(s => ((s.ApprovedAt != null) && s.StoryCommentsInternal.Any(c2 => c2.User.Id == id))).Select(s => s.Id).Any(i => i == c1.Story.Id))
                                          .OrderByDescending(c3 => c3.CreatedAt)
                                          .Select(c4 => c4.Story)
                                          .Skip(start).Take(max));

        private static readonly Func<Database, int>
            CountByPublishedQuery = CompiledQuery.Compile<Database, int>(
                db=>db.StorySet.Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null)));

        private static readonly Func<Database, int>
            CountByUpcomingQuery = CompiledQuery.Compile<Database, int>(
                db => db.StorySet
                        .Count(s => (s.ApprovedAt != null) && (s.PublishedAt == null)));

        private static readonly Func<Database, Guid, int>
            CountByCategoryQuery = CompiledQuery.Compile<Database, Guid, int>(
                (db, id) => db.StorySet
                              .Count(s => (s.ApprovedAt != null) && (s.PublishedAt != null) && (s.Category.Id == id)));

        private static readonly Func<Database, Guid, int>
            CountByTagQuery = CompiledQuery.Compile<Database, Guid, int>(
                (db, id) => db.StorySet
                              .Count(s => (s.ApprovedAt != null) && s.StoryTagsInternal.Any(t => t.Id == id)));

        private static readonly Func<Database, int>
            CountByNewQuery = CompiledQuery.Compile<Database, int>(
                db => db.StorySet
                        .Count(s => ((s.ApprovedAt != null) && (s.LastProcessedAt == null))));

        private static readonly Func<Database, DateTime, DateTime, int>
            CountByPublishableQuery = CompiledQuery.Compile<Database, DateTime, DateTime, int>(
                (db, minDate, maxDate) => db.StorySet
                                            .Count(s => (((s.ApprovedAt >= minDate) && (s.ApprovedAt <= maxDate)) && ((s.LastProcessedAt == null) || (s.LastProcessedAt <= s.LastActivityAt)))));
        
        private static readonly Func<Database, Guid, int>
            CountPostedByUserQuery = CompiledQuery.Compile<Database, Guid, int>(
                (db, id) => db.StorySet
                              .Count(s => (s.ApprovedAt != null) && (s.User.Id == id)));

        private struct FindPublishableQueryParameter
        {
            internal DateTime maximumDate;
            internal DateTime minimumDate;
            internal int start;
            internal int max;
        }
    }
}
