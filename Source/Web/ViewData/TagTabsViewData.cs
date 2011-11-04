namespace Kigg.Web
{
    using System.Collections.Generic;

    using Domain.Entities;

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