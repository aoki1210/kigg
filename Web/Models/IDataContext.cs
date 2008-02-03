namespace Kigg
{
    using System;

    /// <summary>
    /// Data Acess Interface.
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        Category[] GetCategories();

        /// <summary>
        /// Gets the category for the specified name.
        /// </summary>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns></returns>
        Category GetCategoryByName(string categoryName);

        /// <summary>
        /// Gets the tag for the specified name.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        Tag GetTagByName(string tagName);

        /// <summary>
        /// Gets the top tags.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        TagItem[] GetTags(int top);

        /// <summary>
        /// Gets the story detail by id.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="storyId">The story id.</param>
        /// <returns></returns>
        StoryDetailItem GetStoryDetailById(Guid userId, int storyId);

        /// <summary>
        /// Gets the published stories for all category.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        StoryListItem[] GetPublishedStoriesForAllCategory(Guid userId, int start, int max, out int total);

        /// <summary>
        /// Gets the published stories for the specified category.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        StoryListItem[] GetPublishedStoriesForCategory(Guid userId, int categoryId, int start, int max, out int total);

        /// <summary>
        /// Gets the stories for the specified tag.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="tagId">The tag id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        StoryListItem[] GetStoriesForTag(Guid userId, int tagId, int start, int max, out int total);

        /// <summary>
        /// Gets the upcoming stories.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        StoryListItem[] GetUpcomingStories(Guid userId, int start, int max, out int total);

        /// <summary>
        /// Gets the stories posted by the specified user.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="postedByUserId">The posted by user id.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        StoryListItem[] GetStoriesPostedByUser(Guid userId, Guid postedByUserId, int start, int max, out int total);

        /// <summary>
        /// Searches the stories.
        /// </summary>
        /// <param name="userId">The currently visiting user id.</param>
        /// <param name="query">The query.</param>
        /// <param name="start">The start.</param>
        /// <param name="max">The max.</param>
        /// <param name="total">The total.</param>
        /// <returns></returns>
        StoryListItem[] SearchStories(Guid userId, string query, int start, int max, out int total);

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
        int SubmitStory(string url, string title, int categoryId, string description, string tags, Guid userId);

        /// <summary>
        /// Kiggs the specified story.
        /// </summary>
        /// <param name="storyId">The story id.</param>
        /// <param name="userId">The user id who is kigging the story.</param>
        /// <param name="qualifyingKigg">The qualifying kigg which is considered to mark a story as published.</param>
        void KiggStory(int storyId, Guid userId, int qualifyingKigg);

        /// <summary>
        /// Post comments for the specified story.
        /// </summary>
        /// <param name="storyId">The story id.</param>
        /// <param name="userId">The user id who is posting the comment.</param>
        /// <param name="content">The content.</param>
        /// <returns>Returns the id of the new comment</returns>
        int PostComment(int storyId, Guid userId, string content);
    }
}
