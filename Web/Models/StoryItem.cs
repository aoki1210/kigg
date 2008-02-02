namespace Kigg
{
    using System;

    public abstract class StoryItem
    {
        public int ID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public string[] Tags
        {
            get;
            set;
        }

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

        public DateTime? PublishedOn
        {
            get;
            set;
        }

        public bool HasVoted
        {
            get;
            set;
        }

        public bool IsPublished
        {
            get
            {
                return PublishedOn.HasValue;
            }
        }

        public string PostedAgo
        {
            get
            {
                return PostedOn.Ago();
            }
        }

        public string PublishedAgo
        {
            get
            {
                if (IsPublished)
                {
                    return PublishedOn.Value.Ago();
                }

                return string.Empty;
            }
        }

        public int VoteCount
        {
            get;
            set;
        }
    }
}