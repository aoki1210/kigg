namespace Kigg
{
    public class StoryListItem : StoryItem
    {
        public int CommentCount
        {
            get;
            set;
        }

        public string StrippedDescription
        {
            get
            {
                if (Description.Length < 255)
                {
                    return Description;
                }

                return string.Concat(Description.Substring(0, 252), "...");
            }
        }
    }
}