namespace Kigg.Web
{
    using System.Web;

    public abstract class BaseHttpModule : DisposableResource, IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => OnBeginRequest(new HttpContextWrapper(((HttpApplication) sender).Context));
            context.EndRequest += (sender, e) => OnBeginRequest(new HttpContextWrapper(((HttpApplication) sender).Context));
        }

        public virtual void OnBeginRequest(HttpContextBase context)
        {
        }

        public virtual void OnEndRequest(HttpContextBase context)
        {
        }
    }
}