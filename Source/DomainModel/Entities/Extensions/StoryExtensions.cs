namespace Kigg.Domain.Entities
{
    using System;
    using System.Diagnostics;

    public static class StoryExtensions
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
            throw new NotImplementedException();
            /*IConfigurationSettings settings = IoC.Resolve<IConfigurationSettings>();

            DateTime max = SystemTime.Now.AddHours(-settings.MaximumAgeOfStoryInHoursToPublish);

            return story.CreatedAt <= max;*/
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
            throw new NotImplementedException();
            //return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Small);
        }

        [DebuggerStepThrough]
        public static string MediumThumbnail(this Story story)
        {
            throw new NotImplementedException();
            //return IoC.Resolve<IThumbnail>().For(story.Url, ThumbnailSize.Medium);
        }
    }
}