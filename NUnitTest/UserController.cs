namespace Kigg.NUnitTest
{
    using System.Web.Security;

    using NUnit.Framework;
    using Rhino.Mocks;

    using Kigg;

    [TestFixture]
    public class UserControllerTest
    {
        private const string DefaultUserName = "foobar";
        private const string DefaultEmail = "foo@bar.com";
        private const string DefaultPassword = "foobar";
        private const int DefaultPasswordLength = 4;

        private MockRepository mocks;
        private MembershipProvider userManager;
        private UserController controller;
        private MockViewEngine viewEngine;

        [SetUp]
        public void Init()
        {
            mocks = new MockRepository();
            userManager = mocks.PartialMock<MembershipProvider>();
            viewEngine = new MockViewEngine();
            controller = new UserController(userManager) { ViewEngine = viewEngine };
        }

        [Test]
        public void ShouldLogin()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(userManager.ValidateUser(DefaultUserName, DefaultPassword)).IgnoreArguments().Return(true);
            }

            using (mocks.Playback())
            {
                controller.Login(DefaultUserName, DefaultPassword, true);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsTrue(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.IsNull(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage);
            }
        }

        [Test]
        public void ShouldNotLoginForEmptyUserName()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
            }

            using (mocks.Playback())
            {
                controller.Login(string.Empty, DefaultPassword, false);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "User name cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotLoginForEmptyPassword()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
            }

            using (mocks.Playback())
            {
                controller.Login(DefaultUserName, string.Empty, false);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Password cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotLoginForInvaildUser()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                Expect.Call(userManager.ValidateUser(DefaultUserName, DefaultPassword)).IgnoreArguments().Return(false);
            }

            using (mocks.Playback())
            {
                controller.Login(DefaultUserName, DefaultPassword, false);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Invalid login credentials.");
            }
        }

        [Test]
        public void ShouldLogoutForLoggedInUser()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, true);
            }

            using (mocks.Playback())
            {
                controller.Logout();

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsTrue(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.IsNull(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage);
            }
        }

        [Test]
        public void ShouldNotLogoutForNonLoggedInUser()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller, false);
            }

            using (mocks.Playback())
            {
                controller.Logout();

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "You are not logged in.");
            }
        }

        [Test]
        public void ShouldSendPassword()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                var user = mocks.Stub<MembershipUser>();
                SetupResult.For(user.UserName).Return(DefaultUserName);

                Expect.Call(user.ResetPassword()).IgnoreArguments().Return(DefaultPassword);

                Expect.Call(userManager.GetUserNameByEmail(DefaultEmail)).IgnoreArguments().Return(DefaultUserName);
                Expect.Call(userManager.GetUser(DefaultUserName, false)).IgnoreArguments().Return(user);
            }

            using (mocks.Playback())
            {
                controller.SendPassword(DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsTrue(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.IsNull(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage);
            }
        }

        [Test]
        public void ShouldNotSendPasswordForEmptyEmail()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
            }

            using (mocks.Playback())
            {
                controller.SendPassword(string.Empty);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Email cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotSendPasswordForInvalidEmail()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
                Expect.Call(userManager.GetUserNameByEmail(DefaultEmail)).IgnoreArguments().Return(null);
            }

            using (mocks.Playback())
            {
                controller.SendPassword(DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Did not find any user for specified email.");
            }
        }

        [Test]
        public void ShouldNotSendPasswordForInvalidEmailFormat()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
            }

            using (mocks.Playback())
            {
                controller.SendPassword("foo");

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Invalid email address.");
            }
        }

        [Test]
        public void ShouldSignup()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                var user = mocks.Stub<MembershipUser>();
                SetupResult.For(user.UserName).Return(DefaultUserName);
                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);

                MembershipCreateStatus status;

                Expect.Call(userManager.CreateUser(DefaultUserName, DefaultPassword, DefaultEmail, null, null, true, null, out status)).IgnoreArguments().Return(user);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsTrue(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.IsNull(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage);
            }
        }

        [Test]
        public void ShouldNotSignupForEmptyUserName()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
            }

            using (mocks.Playback())
            {
                controller.Signup(string.Empty, DefaultPassword, DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "User name cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotSignupForEmptyPassword()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, string.Empty, DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Password cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotSignupForInvalidPasswordLength()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, "foo", DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, string.Format(System.Globalization.CultureInfo.InvariantCulture, "Password must be {0} character long.", DefaultPasswordLength));
            }
        }

        [Test]
        public void ShouldNotSignupForEmptyEmail()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, string.Empty);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Email cannot be blank.");
            }
        }

        [Test]
        public void ShouldNotSignupForInvalidEmail()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, "foo");

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "Invalid email address.");
            }
        }

        [Test]
        public void ShouldNotSignupForDuplicateUserName()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);

                MembershipCreateStatus status;

                Expect.Call(userManager.CreateUser(DefaultUserName, DefaultPassword, DefaultEmail, null, null, true, null, out status)).IgnoreArguments().OutRef(MembershipCreateStatus.DuplicateUserName).Return(null);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "The username is already in use.");
            }
        }

        [Test]
        public void ShouldNotSignupForDuplicateEmail()
        {
            using (mocks.Record())
            {
                mocks.MockControllerContext(controller);

                SetupResult.For(userManager.MinRequiredPasswordLength).Return(DefaultPasswordLength);

                MembershipCreateStatus status;

                Expect.Call(userManager.CreateUser(DefaultUserName, DefaultPassword, DefaultEmail, null, null, true, null, out status)).IgnoreArguments().OutRef(MembershipCreateStatus.DuplicateEmail).Return(null);
            }

            using (mocks.Playback())
            {
                controller.Signup(DefaultUserName, DefaultPassword, DefaultEmail);

                Assert.AreEqual(viewEngine.ViewContext.ViewName, "Json");
                Assert.IsInstanceOfType(typeof(JsonResult), viewEngine.ViewContext.ViewData);
                Assert.IsFalse(((JsonResult)viewEngine.ViewContext.ViewData).isSuccessful);
                Assert.AreEqual(((JsonResult)viewEngine.ViewContext.ViewData).errorMessage, "The E-mail address is already in use.");
            }
        }
    }
}