using System.IO.Compression;

namespace Kigg
{
    using System.Web;
    using System.Web.Mvc;

    public class CompressFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(FilterExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            //It seems the Abstraction Response does not implement the AddHeader
            //of the original response object, maybe in future we will have it.

            //So we cant use the following.
            //var response = filterContext.HttpContext.Response;

            //Instead we have to use the Old HttpContext
            if (HttpContext.Current == null) return;

            var response = HttpContext.Current.Response;

            var acceptEncoding = request.Headers["Accept-Encoding"];

            if (!string.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToUpperInvariant();

                if (acceptEncoding.Contains("GZIP"))
                {
                    response.AddHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("DEFLATE"))
                {
                    response.AddHeader("Content-encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                }
            }
        }
    }
}