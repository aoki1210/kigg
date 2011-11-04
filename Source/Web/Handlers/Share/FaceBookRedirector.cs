namespace Kigg.Web
{
    using System.Web;

    using Domain.Entities;

    public class FaceBookRedirector : ISocialServiceRedirector
    {
        public void Redirect(HttpContextBase httpContext, Story story)
        {
            httpContext.Response.Redirect("http://www.facebook.com/sharer.php?u={0}".FormatWith(story.Url.UrlEncode()));
        }
    }
}