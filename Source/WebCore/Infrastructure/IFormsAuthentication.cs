namespace Kigg.Web.Security
{
    public interface IFormsAuthentication
    {
        string LogOnUrl
        {
            get;
        }

        void SetAuthenticationCookie(string userName, bool createPersistentCookie);

        void LogOff();
    }
}