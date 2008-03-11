namespace Kigg
{
    using System;

    public abstract class BaseStoryListData : BaseViewData
    {
        public int StoryPerPage
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }

        public StoryListItem[] Stories
        {
            get;
            set;
        }

        public int StoryCount
        {
            get;
            set;
        }

        public int PageCount
        {
            get
            {
                if ((StoryCount == 0) || (StoryPerPage == 0))
                {
                    return 1;
                }

                if ((StoryCount % StoryPerPage) == 0)
                {
                    return (StoryCount / StoryPerPage);
                }

                double result = (StoryCount / StoryPerPage);

                result = Math.Ceiling(result);

                return (Convert.ToInt32(result) + 1);
            }
        }
    }
}