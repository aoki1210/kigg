namespace Kigg.Web
{
    using Domain.Entities;

    public class UserMenuViewData
    {
        public bool IsUserAuthenticated
        {
            get;
            set;
        }

        public User CurrentUser
        {
            get;
            set;
        }
    }
}