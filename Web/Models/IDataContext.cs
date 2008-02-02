namespace Kigg
{
    using System;

    public interface IDataContext : IDisposable
    {
        Category[] GetCategories();
        Category GetCategoryByName(string categoryName);
        StoryListItem[] GetPublishedStoriesForAllCategory(Guid userId, int start, int max, out int total);
        StoryListItem[] GetPublishedStoriesForCategory(Guid userId, int categoryId, int start, int max, out int total);
        StoryListItem[] GetStoriesForTag(Guid userId, int tagId, int start, int max, out int total);
        StoryListItem[] GetStoriesPostedByUser(Guid userId, Guid postedByUserId, int start, int max, out int total);
        StoryDetailItem GetStoryDetailById(Guid userId, int storyId);
        Tag GetTagByName(string tagName);
        TagItem[] GetTags(int top);
        StoryListItem[] GetUpcomingStories(Guid userId, int start, int max, out int total);
        void KiggStory(int storyId, Guid userId, int qualifyingKigg);
        void PostComment(int storyId, Guid userId, string content);
        StoryListItem[] SearchStories(Guid userId, string query, int start, int max, out int total);
        void SubmitStory(string url, string title, int categoryId, string description, string tags, Guid userId);
    }
}
