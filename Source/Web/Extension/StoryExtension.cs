namespace Kigg.Web
{
    using Domain.Entities;

    public static class StoryExtension
    {
        public static string StrippedDescription(this Story story)
        {
            return story.TextDescription.WrapAt(512);
        }
    }
}