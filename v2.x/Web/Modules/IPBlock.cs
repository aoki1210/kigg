namespace Kigg.Web
{
    using System;
    using System.Web;

    using Infrastructure;

    public class IPBlock : BaseHttpModule
    {
        public override void OnBeginRequest(HttpContextBase context)
        {
            var requestedUrl = context.Request.Url;

            string prefix = "{0}://{1}/Assets".FormatWith(requestedUrl.Scheme, requestedUrl.Host);

            if (!requestedUrl.ToString().StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                string ip = context.Request.UserHostAddress;

                if (!string.IsNullOrEmpty(ip))
                {
                    bool shouldBlock = IoC.Resolve<IBlockedIPCollection>().Contains(ip);

                    if (shouldBlock)
                    {
                        Log.Warning("Blocked Ip Address detected: {0}.", ip);
                        context.RewritePath("~/Maintenance/IpBlocked.aspx");
                    }
                }
            }
        }
    }
}