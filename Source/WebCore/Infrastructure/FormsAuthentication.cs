namespace Kigg.Web.Security
{
    using System.Diagnostics;
    using Authentication = System.Web.Security.FormsAuthentication;

    public class FormsAuthentication : IFormsAuthentication
    {
        public string LogOnUrl
        {
            [DebuggerStepThrough]
            get
            {
                return Authentication.LoginUrl;
            }
        }

        [DebuggerStepThrough]
        public void SetAuthenticationCookie(string userName, bool createPersistentCookie)
        {
            Authentication.SetAuthCookie(userName, createPersistentCookie);
        }

        [DebuggerStepThrough]
        public void LogOff()
        {
            Authentication.SignOut();
        }
    }
}