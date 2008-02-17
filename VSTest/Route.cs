namespace Kigg.VSTest
{
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using Kigg;

    [TestClass]
    public class RouteTest
    {
        private RouteCollection routes;
        private MockRepository mocks;

        [TestInitialize]
        public void Init()
        {
            routes = new RouteCollection();
            Global.RegisterRoutes(routes);

            mocks = new MockRepository();
        }

        [TestMethod]
        public void VerifyDefault()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Default.aspx");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Category", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyAllCategory()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Category/20");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Category", routeData.Values["action"]);
                Assert.IsNull(routeData.Values["name"]);
                Assert.AreEqual("20", routeData.Values["page"]);
            }
        }

        [TestMethod]
        public void VerifySpecificCategory()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Category/Technology/1");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Category", routeData.Values["action"]);
                Assert.AreEqual("Technology", routeData.Values["name"]);
                Assert.AreEqual("1", routeData.Values["page"]);
            }
        }

        [TestMethod]
        public void VerifyUpcoming()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Upcoming");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Upcoming", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyTag()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Tag/Apple/2");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Tag", routeData.Values["action"]);
                Assert.AreEqual("Apple", routeData.Values["name"]);
                Assert.AreEqual("2", routeData.Values["page"]);
            }
        }

        [TestMethod]
        public void VerifyPostedBy()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/PostedBy/Admin/1");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("PostedBy", routeData.Values["action"]);
                Assert.AreEqual("Admin", routeData.Values["name"]);
                Assert.AreEqual("1", routeData.Values["page"]);
            }
        }

        [TestMethod]
        public void VerifySearch()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Search/apple/5");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Search", routeData.Values["action"]);
                Assert.AreEqual("apple", routeData.Values["q"]);
                Assert.AreEqual("5", routeData.Values["page"]);
            }
        }

        [TestMethod]
        public void VerifyDetail()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Detail/1000");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Detail", routeData.Values["action"]);
                Assert.AreEqual("1000", routeData.Values["id"]);
            }
        }

        [TestMethod]
        public void VerifySubmit()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Submit");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Submit", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyKigg()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Kigg");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Kigg", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyComment()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/Story/Comment");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Comment", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyLogin()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/User/Login");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("Login", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyLogout()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/User/Logout");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("Logout", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifySendPassword()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/User/SendPassword");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("SendPassword", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifySignup()
        {
            IHttpContext httpContext;

            using (mocks.Record())
            {
                httpContext = GetHttpContext(mocks, "~/User/Signup");
            }

            using (mocks.Playback())
            {
                RouteData routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("Signup", routeData.Values["action"]);
            }
        }

        private static IHttpContext GetHttpContext(MockRepository mocks, string url)
        {
            IHttpContext httpContext = mocks.DynamicMock<IHttpContext>();
            IHttpRequest httpRequest = mocks.DynamicMock<IHttpRequest>();
            IHttpResponse httpResponse = mocks.DynamicMock<IHttpResponse>();
            IHttpSessionState httpSession = mocks.DynamicMock<IHttpSessionState>();
            IHttpServerUtility httpServer = mocks.DynamicMock<IHttpServerUtility>();

            SetupResult.For(httpContext.Request).Return(httpRequest);
            SetupResult.For(httpContext.Response).Return(httpResponse);
            SetupResult.For(httpContext.Session).Return(httpSession);
            SetupResult.For(httpContext.Server).Return(httpServer);

            mocks.Replay(httpContext);

            SetupResult.For(httpContext.Request.AppRelativeCurrentExecutionFilePath).Return(url);

            return httpContext;
        }
    }
}