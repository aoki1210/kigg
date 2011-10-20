namespace Kigg.Web
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using DomainObjects;

    public class StoryListViewData : BaseStoryViewData
    {
        public StoryListViewData()
        {
            Stories = new ReadOnlyCollection<Story>(new List<Story>());
        }

        public int PageCount
        {
            [DebuggerStepThrough]
            get
            {
                return PageCalculator.TotalPage(TotalStoryCount, StoryPerPage);
            }
        }

        public string Title
        {
            get;
            set;
        }

        public string RssUrl
        {
            get;
            set;
        }

        public string AtomUrl
        {
            get;
            set;
        }

        public string Subtitle
        {
            get;
            set;
        }

        public string NoStoryExistMessage
        {
            get;
            set;
        }

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

        public ICollection<Story> Stories
        {
            get;
            set;
        }

        public int TotalStoryCount
        {
            get;
            set;
        }
    }
}