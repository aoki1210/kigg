namespace Kigg.Web
{
    using System.Collections.Generic;

    using Domain.Entities;

    public class UserDetailViewData : BaseViewData
    {
        public User TheUser
        {
            get;
            set;
        }

        public IDictionary<string, bool> IPAddresses
        {
            get;
            set;
        }

        public UserDetailTab SelectedTab
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }
    }
}