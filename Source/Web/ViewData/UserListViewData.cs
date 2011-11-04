namespace Kigg.Web
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    using Domain.Entities;

    public class UserListViewData : BaseViewData
    {
        public UserListViewData()
        {
            Users = new ReadOnlyCollection<User>(new List<User>());
        }

        public int PageCount
        {
            [DebuggerStepThrough]
            get
            {
                return PageCalculator.TotalPage(TotalUserCount, UserPerPage);
            }
        }

        public string Title
        {
            get;
            set;
        }

        public string Subtitle
        {
            get;
            set;
        }

        public string NoUserExistMessage
        {
            get;
            set;
        }

        public int UserPerPage
        {
            get;
            set;
        }

        public int CurrentPage
        {
            get;
            set;
        }

        public ICollection<User> Users
        {
            get;
            set;
        }

        public int TotalUserCount
        {
            get;
            set;
        }
    }
}