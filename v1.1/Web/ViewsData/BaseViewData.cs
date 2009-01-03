namespace Kigg
{
    public abstract class BaseViewData
    {
        public string UserName
        {
            get;
            set;
        }

        public bool IsAuthenticated
        {
            get;
            set;
        }

        public Category[] Categories
        {
            get;
            set;
        }

        public TagItem[] Tags
        {
            get;
            set;
        }

        public string SearchQuery
        {
            get;
            set;
        }
    }
}