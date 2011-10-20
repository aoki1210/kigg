namespace Kigg.Web
{
    using DomainObjects;

    public static class StoryExtension
    {
        public static string StrippedDescription(this Story story)
        {
            return story.TextDescription.WrapAt(512);
        }
    }
}