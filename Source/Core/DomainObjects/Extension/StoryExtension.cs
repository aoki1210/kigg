namespace Kigg.DomainObjects
{
    using System;
    using System.Diagnostics;

    using Infrastructure;

    public static class StoryExtension
    {
        [DebuggerStepThrough]
        public static bool IsNew(this Story story)
        {
            return !story.LastProcessedAt.HasValue;
        }

        [DebuggerStepThrough]
        public static bool IsPublished(this Story story)
        {
            return story.PublishedAt.HasValue;
        }

        [DebuggerStepThrough]
        public static bool HasExpired(this Story story)
        {
            IConfigurationSettings settings = IoC.Resolve<IConfigurationSettings>();

            DateTime max = SystemTime.Now().AddHours(-settings.MaximumAgeOfStoryInHoursToPublish);

            return story.CreatedAt <= max;
        }

        [DebuggerStepThrough]
        public static bool IsApproved(this Story story)
        {
            return story.ApprovedAt.HasValue;
        }
        
        [DebuggerStepThrough]
        public static bool IsPostedBy(this Story story, User byUser)
        {
            return (byUser != null) && (story.PostedBy.Id == byUser.Id);
        }

        [DebuggerStepThrough]
        public static string Host(this Story story)
        {
            return new Uri(story.Url).Host;
        }

        [DebuggerStepThrough]
        public static string SmallThumbnail(this Story story)
        {
            return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Small);
        }

        [DebuggerStepThrough]
        public static string MediumThumbnail(this Story story)
        {
            return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Medium);
        }
    }
}