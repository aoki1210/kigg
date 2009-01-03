namespace Kigg
{
    public class StoryDetailItem : StoryItem
    {
        public UserItem[] VotedBy
        {
            get;
            set;
        }

        public CommentItem[] Comments
        {
            get;
            set;
        }
    }
}