namespace Kigg.Web
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AppendOpenIdXrdsLocationAttribute : FilterAttribute, IResultFilter
    {
        public AppendOpenIdXrdsLocationAttribute()
        {
            Order = int.MaxValue;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Do nothing, I'm gonna take nap :0)
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            UrlHelper urlHelper = filterContext.RequestContext.UrlHelper();
            string url = urlHelper.ToAbsolute(urlHelper.Xrds());

            filterContext.HttpContext.Response.AddHeader("X-XRDS-Location", url);
        }
    }
}