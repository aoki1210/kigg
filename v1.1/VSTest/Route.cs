﻿namespace Kigg.VSTest
{
    using System.Web;
    using System.Web.Routing;
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
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Default.aspx");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Category", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyAllCategory()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Category/20");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

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
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Category/Technology/1");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

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
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Upcoming");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Upcoming", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyTag()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Tag/Apple/2");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

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
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/PostedBy/Admin/1");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

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
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Search/apple/5");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

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
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Detail/1000");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Detail", routeData.Values["action"]);
                Assert.AreEqual("1000", routeData.Values["id"]);
            }
        }

        [TestMethod]
        public void VerifySubmit()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Submit");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Submit", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyKigg()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Kigg");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Kigg", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyComment()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/Story/Comment");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("Story", routeData.Values["Controller"]);
                Assert.AreEqual("Comment", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyLogin()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/User/Login");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("Login", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifyLogout()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/User/Logout");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("Logout", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifySendPassword()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/User/SendPassword");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("SendPassword", routeData.Values["action"]);
            }
        }

        [TestMethod]
        public void VerifySignup()
        {
            HttpContextBase httpContext;

            using (mocks.Record())
            {
                httpContext = mocks.MockHttpContext("~/User/Signup");
            }

            using (mocks.Playback())
            {
                var routeData = routes.GetRouteData(httpContext);

                Assert.IsNotNull(routeData);
                Assert.AreEqual("User", routeData.Values["Controller"]);
                Assert.AreEqual("Signup", routeData.Values["action"]);
            }
        }
    }
}