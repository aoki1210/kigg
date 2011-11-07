namespace Kigg.Web.Security
{
    using System.Web.Mvc;

    [ModelBinder(typeof(OpenIdCommandBinder))]
    public class OpenIdCommand
    {
        public string UserName
        {
            get; set;
        }

        public bool? RememberMe
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }
    }
}