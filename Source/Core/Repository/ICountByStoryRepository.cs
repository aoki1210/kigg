namespace Kigg.Repository
{
    using System;

    public interface ICountByStoryRepository
    {
        int CountByStory(long storyId);
    }
}
