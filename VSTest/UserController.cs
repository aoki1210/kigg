    namespace Kigg.VSTest
{
    using System.Web.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using Kigg;

    [TestClass]
    public class UserControllerTest
    {
        private const string DefaultUserName = "foobar";
        private const string DefaultEmail = "foo@bar.com";
        private const string DefaultPassword = "foobar";
        private const int DefaultPasswordLength = 4;

        private MockRepository mocks = null;
        private MembershipProvider userManager = null;
        private UserControllerForTest controller = null;

        [TestInitialize()]
        public void Init()
        {
            mocks = new MockRepository();
            userManager = mocks.PartialMock<MembershipProvider>();
            controller = new UserControllerForTest(userManager);
        }

        [TestMethod]
        public void ShouldLogin()
        {
            using(mocks.Record())
            {
                Expect.Call(userManager.ValidateUser(DefaultUserName, DefaultPassword)).IgnoreArguments().Return(true);
            }

            using(mocks.Playback())
            {
                controller.Login(DefaultUserName, DefaultPassword, true);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsTrue(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.IsNull(((JsonResult)controller.SelectedViewData).errorMessage);
        }

        [TestMethod]
        public void ShoudNotLoginForEmptyUserName()
        {
            controller.Login(string.Empty, DefaultPassword, false);

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "User name cannot be blank.");
        }

        [TestMethod]
        public void ShoudNotLoginForEmptyPassword()
        {
            controller.Login(DefaultUserName, string.Empty, false);

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Password cannot be blank.");
        }

        [TestMethod]
        public void ShouldNotLoginForInvaildUser()
        {
            using (mocks.Record())
            {
                Expect.Call(userManager.ValidateUser(DefaultUserName, DefaultPassword)).IgnoreArguments().Return(false);
            }

            using (mocks.Playback())
            {
                controller.Login(DefaultUserName, DefaultPassword, false);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Invalid login credentials.");
        }

        [TestMethod]
        public void ShouldSendPassword()
        {
            using (mocks.Record())
            {
                MembershipUser user = mocks.Stub<MembershipUser>();
                SetupResult.For(user.UserName).Return(DefaultUserName);

                Expect.Call(user.ResetPassword()).IgnoreArguments().Return(DefaultPassword);

                Expect.Call(userManager.GetUserNameByEmail(DefaultEmail)).IgnoreArguments().Return(DefaultUserName);
                Expect.Call(userManager.GetUser(DefaultUserName, false)).IgnoreArguments().Return(user);
            }

            using (mocks.Playback())
            {
                controller.SendPassword(DefaultEmail);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsTrue(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.IsNull(((JsonResult)controller.SelectedViewData).errorMessage);
        }

        [TestMethod]
        public void ShoudNotSendPasswordForEmptyEmail()
        {
            controller.SendPassword(string.Empty);

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Email cannot be blank.");
        }

        [TestMethod]
        public void ShoudNotSendPasswordForInvalidEmail()
        {
            controller.SendPassword("foo");

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Invalid email address.");
        }

        [TestMethod]
        public void ShouldSignup()
        {
            using (mocks.Record())
            {
                MembershipUser user = mocks.Stub<MembershipUser>();
                SetupResult.For(user.UserName).Return(DefaultUserName);
                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);

                MembershipCreateStatus status;

                Expect.Call(userManager.CreateUser(DefaultUserName, DefaultPassword, DefaultEmail, null, null, true, null, out status)).IgnoreArguments().Return(user);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, DefaultEmail);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsTrue(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.IsNull(((JsonResult)controller.SelectedViewData).errorMessage);
        }

        [TestMethod]
        public void ShoudNotSignupForEmptyUserName()
        {
            controller.Signup(string.Empty, DefaultPassword, DefaultEmail);

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "User name cannot be blank.");
        }

        [TestMethod]
        public void ShoudNotSignupForEmptyPassword()
        {
            controller.Signup(DefaultUserName, string.Empty, DefaultEmail);

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Password cannot be blank.");
        }

        [TestMethod]
        public void ShoudNotSignupForInvalidPasswordLength()
        {
            using (mocks.Record())
            {
                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, "foo", DefaultEmail);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, string.Format("Password must be {0} character long.", DefaultPasswordLength));
        }

        [TestMethod]
        public void ShoudNotSignupForEmptyEmail()
        {
            using (mocks.Record())
            {
                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, string.Empty);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Email cannot be blank.");
        }

        [TestMethod]
        public void ShoudNotSignupForInvalidEmail()
        {
            using (mocks.Record())
            {
                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, "foo");
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "Invalid email address.");
        }

        [TestMethod]
        public void ShouldNotSignupForDuplicateUserName()
        {
            using (mocks.Record())
            {
                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);

                MembershipCreateStatus status;

                Expect.Call(userManager.CreateUser(DefaultUserName, DefaultPassword, DefaultEmail, null, null, true, null, out status)).IgnoreArguments().OutRef(MembershipCreateStatus.DuplicateUserName).Return(null);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, DefaultEmail);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "The username is already in use.");
        }

        [TestMethod]
        public void ShouldNotSignupForDuplicateEmail()
        {
            using (mocks.Record())
            {
                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);

                MembershipCreateStatus status;

                Expect.Call(userManager.CreateUser(DefaultUserName, DefaultPassword, DefaultEmail, null, null, true, null, out status)).IgnoreArguments().OutRef(MembershipCreateStatus.DuplicateEmail).Return(null);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, DefaultEmail);
            }

            Assert.AreEqual(controller.SelectedView, "Json");
            Assert.IsInstanceOfType(controller.SelectedViewData, typeof(JsonResult));
            Assert.IsFalse(((JsonResult)controller.SelectedViewData).isSuccessful);
            Assert.AreEqual(((JsonResult)controller.SelectedViewData).errorMessage, "The E-mail address is already in use.");
        }

        private class UserControllerForTest : UserController
        {
            public string SelectedView
            {
                get;
                private set;
            }

            public object SelectedViewData
            {
                get;
                private set;
            }

            public UserControllerForTest(MembershipProvider userManager): base(userManager)
            {
            }

            protected override void RenderView(string viewName, string masterName, object viewData)
            {
                SelectedView = viewName;
                SelectedViewData = viewData;
            }
        }
    }
}