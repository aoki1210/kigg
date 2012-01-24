namespace Kigg.Web.Security
{
    public interface IFormsAuthentication
    {
        string LoginUrl
        {
            get;
        }

        void SetAuthenticationCookie(string userName, bool createPersistentCookie);

        void Logout();
    }
}