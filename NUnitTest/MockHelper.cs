namespace Kigg.NUnitTest
{
    using System;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;

    using Rhino.Mocks;

    public static class MvcMockHelpers
    {
        public static HttpContextBase MockHttpContext(this MockRepository mocks)
        {
            var context = mocks.PartialMock<HttpContextBase>();
            var request = mocks.PartialMock<HttpRequestBase>();
            var response = mocks.PartialMock<HttpResponseBase>();
            var session = mocks.PartialMock<HttpSessionStateBase>();
            var server = mocks.PartialMock<HttpServerUtilityBase>();

            SetupResult.For(context.Request).Return(request);
            SetupResult.For(context.Response).Return(response);
            SetupResult.For(context.Session).Return(session);
            SetupResult.For(context.Server).Return(server);

            return context;
        }

        public static HttpContextBase MockHttpContext(this MockRepository mocks, string url)
        {
            var context = MockHttpContext(mocks);
            mocks.Replay(context);

            context.Request.SetupRequestUrl(url);

            return context;
        }

        public static HttpContextBase MockHttpContext(this MockRepository mocks, bool authenticated)
        {
            var context = MockHttpContext(mocks);

            var principal = mocks.DynamicMock<IPrincipal>();
            var identity = mocks.DynamicMock<IIdentity>();

            SetupResult.For(identity.IsAuthenticated).Return(authenticated);
            SetupResult.For(identity.Name).Return(string.Empty);
            SetupResult.For(principal.Identity).Return(identity);
            SetupResult.For(context.User).Return(principal);

            mocks.Replay(context);

            return context;
        }

        public static MembershipProvider MockMembershipProvider(this MockRepository mocks, bool shouldReturnGetUser)
        {
            const string DefaultUserName = "";

            var provider = mocks.PartialMock<MembershipProvider>();

            if (shouldReturnGetUser)
            {
                var user = mocks.Stub<MembershipUser>();

                SetupResult.For(user.ProviderUserKey).Return(Guid.NewGuid());
                SetupResult.For(user.UserName).Return(DefaultUserName);

                SetupResult.For(provider.GetUser(DefaultUserName, true)).IgnoreArguments().Return(user);
            }

            return provider;
        }

        public static void MockControllerContext(this MockRepository mocks, Controller controller, bool authenticated)
        {
            var httpContext = mocks.MockHttpContext(authenticated);
            var context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);

            controller.ControllerContext = context;
        }

        public static void MockControllerContext(this MockRepository mocks, Controller controller)
        {
            var httpContext = mocks.MockHttpContext(false);
            var context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);

            controller.ControllerContext = context;
        }

        public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
        {
            SetupResult.For(request.HttpMethod).Return(httpMethod);
        }

        public static void SetupRequestUrl(this HttpRequestBase request, string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (!url.StartsWith("~/"))
            {
                throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");
            }

            SetupResult.For(request.QueryString).Return(GetQueryStringParameters(url));
            SetupResult.For(request.AppRelativeCurrentExecutionFilePath).Return(GetUrlFileName(url));
            SetupResult.For(request.PathInfo).Return(string.Empty);
        }

        private static string GetUrlFileName(string url)
        {
            if (url.Contains("?"))
            {
                return url.Substring(0, url.IndexOf("?"));
            }

            return url;
        }

        private static NameValueCollection GetQueryStringParameters(string url)
        {
            if (url.Contains("?"))
            {
                var parameters = new NameValueCollection();

                var parts = url.Split('?');
                var keys = parts[1].Split('&');

                foreach (var key in keys)
                {
                    var part = key.Split('=');
                    parameters.Add(part[0], part[1]);
                }

                return parameters;
            }

            return null;
        }
    }
}