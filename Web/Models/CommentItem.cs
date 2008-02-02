namespace Kigg
{
    using System;

    public class CommentItem
    {
        public UserItem PostedBy
        {
            get;
            set;
        }

        public DateTime PostedOn
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public string PostedAgo
        {
            get
            {
                return PostedOn.Ago();
            }
        }
    }
}