namespace Kigg.Web
{
    using System.Web;

    using Domain.Entities;

    public interface ISocialServiceRedirector
    {
        void Redirect(HttpContextBase httpContext, Story story);
    }
}