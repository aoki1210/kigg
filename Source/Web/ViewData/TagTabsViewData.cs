namespace Kigg.Web
{
    using System.Collections.Generic;

    using DomainObjects;

    public class TagTabsViewData
    {
        public IEnumerable<Tag> PopularTags
        {
            get;
            set;
        }

        public IEnumerable<Tag> UserTags
        {
            get;
            set;
        }
    }
}