namespace Kigg.Web
{
    using System.Web;
    using Domain.Entities;

    public class MsdnRedirector : ISocialServiceRedirector
    {
        public void Redirect(HttpContextBase httpContext, Story story)
        {
            httpContext.Response.Redirect("http://social.msdn.microsoft.com/en-US/action/Create/s/E/?url={0}&ttl={1}&d={2}&bm=true".FormatWith(story.Url.UrlEncode(), story.Title.UrlEncode(), story.TextDescription.UrlEncode()));
        }
    }
}