namespace Kigg
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Security.Cryptography;
    using System.Linq;
    using System.Data.Linq;
    using System.Transactions;

    /// <summary>
    /// Linq to Sql implementation of <see cref="IDataContext"/>.
    /// </summary>
    public partial class KiggDataContext : IDataContext
    {
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        public Category[] GetCategories()
        {
            return Categories.OrderBy(c => c.ID).ToArray();
        }

        /// <summary>
        /// Gets the category for the specified name.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns></returns>
        public Category GetCategoryByName(string categoryName)
        {
            return Categories.FirstOrDefault(c => c.Name == categoryName);
        }

        /// <summary>
        /// Gets the tag for the specified name.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        public Tag GetTagByName(string tagName)
        {
            return Tags.FirstOrDefault(t => t.Name == tagName);
        }

        /// <summary>
        /// Gets the top tags.
        /// </summary>
        /// <param name="top">The number of top tags.</param>
        /// <returns></returns>
        public TagItem[] GetTags(int top)
        {
            return Tags
                        .Select
                        (
                           t =>
                           new TagItem
                           {
                               ID = t.ID,
                               Name = t.Name,
                               Count = t.StoryTags.Count()
                           }
                        )
                        .OrderByDescending(t => t.Count)
                        .ThenBy(t => t.Name)
                        .Take(top)
                        .ToArray();
        }

        /// <summary>
        /// Gets the story detail by id.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="storyId">The story id.</param>
        /// <returns></returns>
        public StoryDetailItem GetStoryDetailById(Guid userId, int storyId)
        {
            return Stories
                            .Where(s => s.ID == storyId)
                            .Select(s =>
                                        new StoryDetailItem
                                        {
                                            ID = s.ID,
                                            Title = s.Title,
                                            Description = s.Description,
                                            Url = s.Url,
                                            Category = s.Category.Name,
                                            Tags = s.StoryTags.Select(st => st.Tag.Name).ToArray(),
                                            PostedBy =  new UserItem
                                                        {
                                                            Name = s.User.UserName,
                                                            Email = s.User.UserDetail.Email
                                                        },
                                            PostedOn = s.PostedOn,
                                            PublishedOn = s.PublishedOn,
                                            VoteCount = s.Votes.Count(),
                                            HasVoted = (s.Votes.Count(v => v.UserID == userId) > 0),
                                            VotedBy = s.Votes
                                                            .OrderBy(v => v.Timestamp)
                                                            .Select(v =>
                                                                        new UserItem
                                                                        {
                                                                            Name = v.User.UserName,
                                                                            Email = v.User.UserDetail.Email
                                                                        }
                                                                    )
                                                                    .ToArray(),
                                            Comments = s.Comments
                                                                .OrderBy(c => c.PostedOn)
                                                                .Select(c =>
                                                                            new CommentItem
                                                                            {
                                                                                PostedBy = new UserItem { Name = c.User.UserName, Email = c.User.UserDetail.Email },
                                                                                PostedOn = c.PostedOn,
                                                                                Content = c.Content
                                                                            }
                                                                        ).ToArray()
                                        }
                                    )
                            .FirstOrDefault();
        }

        /// <summary>
        /// Gets the published stories for all category.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public StoryListItem[] GetPublishedStoriesForAllCategory(Guid userId, int start, int max, out int total)
        {
            var stories = Stories.Where(s => s.PublishedOn != null);

            total = stories.Count();

            return PrepareStories(
                                        stories
                                        .OrderByDescending(s => s.PublishedOn),
                                        userId,
                                        start,
                                        max
                                    );
        }

        /// <summary>
        /// Gets the published stories for the specified category.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public StoryListItem[] GetPublishedStoriesForCategory(Guid userId, int categoryId, int start, int max, out int total)
        {
            var stories = Stories.Where(s => (s.PublishedOn != null) && (s.CategoryID == categoryId));
            total = stories.Count();

            return PrepareStories   (
                                        stories
                                        .OrderByDescending(s => s.PublishedOn),
                                        userId,
                                        start,
                                        max
                                    );
        }

        /// <summary>
        /// Gets the stories for the specified tag.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="tagId">The tag id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public StoryListItem[] GetStoriesForTag(Guid userId, int tagId, int start, int max, out int total)
        {
            var stories = Stories.Where(s => s.StoryTags.Count(st => st.TagID == tagId) > 0);

            total = stories.Count();

            //The Published will appear first
            return PrepareStories(
                                    stories
                                    .OrderByDescending(s => s.PublishedOn)
                                    .ThenByDescending(s => s.PostedOn),
                                    userId,
                                    start,
                                    max
                                );
        }

        /// <summary>
        /// Gets the upcoming stories.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public StoryListItem[] GetUpcomingStories(Guid userId, int start, int max, out int total)
        {
            var stories = Stories.Where(s => s.PublishedOn == null);

            total = stories.Count();

            return PrepareStories(  stories
                                    .OrderByDescending(s => s.PostedOn),
                                    userId,
                                    start,
                                    max
                                );
        }

        /// <summary>
        /// Gets the stories posted by the specified user.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="postedByUserId">The posted by user id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public StoryListItem[] GetStoriesPostedByUser(Guid userId, Guid postedByUserId, int start, int max, out int total)
        {
            var stories = Stories.Where(s => s.PostedBy == postedByUserId);

            total = stories.Count();

            return PrepareStories(
                                    stories
                                    .OrderByDescending(s => s.PostedOn),
                                    userId,
                                    start,
                                    max
                                );
        }

        /// <summary>
        /// Searches the stories.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="query">The query.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        public StoryListItem[] SearchStories(Guid userId, string query, int start, int max, out int total)
        {
            var stories = Stories
                                .Where(
                                            s =>
                                            (s.Title.Contains(query)) ||
                                            (s.Description.Contains(query)) ||
                                            (s.Category.Name.Contains(query)) ||
                                            (s.StoryTags.Count(st => st.Tag.Name.Contains(query)) > 0)
                                      );
            total = stories.Count();

            return PrepareStories(
                                    stories
                                    .OrderByDescending(s => s.PostedOn),
                                    userId,
                                    start,
                                    max
                                );
        }

        private static StoryListItem[] PrepareStories(IQueryable<Story> stories, Guid userId, int start, int max)
        {
            return  stories
                            .Select
                            (
                               s =>
                               new StoryListItem
                               {
                                   ID = s.ID,
                                   Title = s.Title,
                                   Description = s.Description,
                                   Url = s.Url,
                                   Category = s.Category.Name,
                                   Tags = s.StoryTags.Select(st => st.Tag.Name).ToArray(),
                                   PostedBy = new UserItem
                                              {
                                                  Name = s.User.UserName,
                                                  Email = s.User.UserDetail.Email
                                              },
                                   PostedOn = s.PostedOn,
                                   PublishedOn = s.PublishedOn,
                                   VoteCount = s.Votes.Count(),
                                   HasVoted = (s.Votes.Count(v => v.UserID == userId) > 0),
                                   CommentCount = s.Comments.Count()
                               }
                            )
                            .Skip(start)
                            .Take(max)
                            .ToArray();
        }

        /// <summary>
        /// Submit the story.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="description">The description.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="userId">The user id who is submitting the stroy.</param>
        /// <returns>Returns the id of the new story</returns>
        public int SubmitStory(string url, string title, int categoryId, string description, string tags, Guid userId)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                Category category = Categories.FirstOrDefault(c => c.ID == categoryId);

                if (category == null)
                {
                    throw new InvalidOperationException("Specified category does not exist.");
                }

                string urlHash = string.Empty;

                using (MD5 md5 = MD5.Create())
                {
                    byte[] data = Encoding.Default.GetBytes(url.ToUpperInvariant());
                    byte[] hash = md5.ComputeHash(data);

                    urlHash = Convert.ToBase64String(hash);
                }

                Story story = Stories.FirstOrDefault(s => s.UrlHash == urlHash);

                if (story != null)
                {
                    throw new InvalidOperationException("Specified story already exists.");
                }

                story = new Story();
                DateTime now = DateTime.UtcNow;

                story.Url = url;
                story.UrlHash = urlHash;
                story.Title = title.Trim();
                story.Description = description.Trim();
                story.CategoryID = categoryId;
                story.PostedBy = userId;
                story.PostedOn = now;
                Stories.InsertOnSubmit(story);
                SubmitChanges();

                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tagsArray = tags.Split(',');

                    if (tagsArray.Length > 0)
                    {
                        List<Tag> newTags = new List<Tag>();
                        List<Tag> allTags = new List<Tag>();

                        for (int i = 0; i < tagsArray.Length; i++)
                        {
                            string tagName = tagsArray[i].Trim();

                            if (tagName.Length > 0)
                            {
                                Tag tag = Tags.FirstOrDefault(t => t.Name == tagName);

                                if (tag == null)
                                {
                                    tag = new Tag { Name = tagName };
                                    newTags.Add(tag);
                                }

                                allTags.Add(tag);
                            }
                        }

                        if (newTags.Count > 0)
                        {
                            Tags.InsertAllOnSubmit(newTags);
                            SubmitChanges();
                        }

                        if (allTags.Count > 0)
                        {
                            List<StoryTag> storyTags = new List<StoryTag>();

                            for (int i = 0; i < allTags.Count; i++)
                            {
                                storyTags.Add(new StoryTag { StoryID = story.ID, TagID = allTags[i].ID });
                            }

                            StoryTags.InsertAllOnSubmit(storyTags);
                            SubmitChanges();
                        }
                    }
                }

                Vote vote = new Vote();
                vote.StoryID = story.ID;
                vote.UserID = userId;
                vote.Timestamp = now;
                Votes.InsertOnSubmit(vote);

                SubmitChanges();

                ts.Complete();

                return story.ID;
            }
        }

        /// <summary>
        /// Kiggs the specified story.
        /// </summary>
        /// <param name="storyId">The story id.</param>
        /// <param name="userId">The user id who is kigging the story.</param>
        /// <param name="qualifyingKigg">The qualifying kigg which is considered to mark a story as published.</param>
        public void KiggStory(int storyId, Guid userId, int qualifyingKigg)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                Story story = Stories.FirstOrDefault(s => s.ID == storyId);

                if (story == null)
                {
                    throw new InvalidOperationException("Specified story does not exist.");
                }

                Vote vote = Votes.FirstOrDefault(v => v.StoryID == storyId && v.UserID == userId);

                if (vote == null)
                {
                    int voteCount = Votes.Where(v => v.StoryID == storyId).Count();
                    DateTime now = DateTime.UtcNow;

                    if ((voteCount + 1) == qualifyingKigg)
                    {
                        story.PublishedOn = now;
                    }

                    vote = new Vote();
                    vote.StoryID = storyId;
                    vote.UserID = userId;
                    vote.Timestamp = now;

                    Votes.InsertOnSubmit(vote);
                    SubmitChanges();
                    ts.Complete();
                }
            }
        }

        /// <summary>
        /// Post comments for the specified story.
        /// </summary>
        /// <param name="storyId">The story id.</param>
        /// <param name="userId">The user id who is posting the comment.</param>
        /// <param name="content">The content.</param>
        /// <returns>Returns the id of the new comment</returns>
        public int PostComment(int storyId, Guid userId, string content)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                Story story = Stories.FirstOrDefault(s => s.ID == storyId);

                if (story == null)
                {
                    throw new InvalidOperationException("Specified story does not exist.");
                }

                Comment comment = new Comment();

                comment.StoryID = storyId;
                comment.Content = content.Trim();
                comment.PostedBy = userId;
                comment.PostedOn = DateTime.UtcNow;

                Comments.InsertOnSubmit(comment);
                SubmitChanges();

                ts.Complete();

                return comment.ID;
            }
        }
    }
}