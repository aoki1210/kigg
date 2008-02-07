namespace Kigg
{
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// Handles all Membership related operations.
    /// </summary>
    public class UserController : BaseController
    {
        private static readonly Regex EmailExpression = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Web.Security.MembershipProvider"/> class.
        /// </summary>
        /// <param name="userManager">The membership provider.</param>
        public UserController(MembershipProvider userManager): base(userManager)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController()
        {
        }

        /// <summary>
        /// Logins the user. This is an Ajax Operation.
        /// </summary>
        /// <param name="userName">Name of the user (Mandatory).</param>
        /// <param name="password">The password (Mandatory).</param>
        /// <param name="rememberMe">If <c>true</c> a persistent cookie is generated.</param>
        [ControllerAction]
        public void Login(string userName, string password, bool rememberMe)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (string.IsNullOrEmpty(userName))
                {
                    result.errorMessage = "User name cannot be blank.";
                }
                else if (string.IsNullOrEmpty(password))
                {
                    result.errorMessage = "Password cannot be blank.";
                }
                else if (!UserManager.ValidateUser(userName, password))
                {
                    result.errorMessage = "Invalid login credentials.";
                }
                else
                {
                    //The following check is required for TDD 
                    if (HttpContext != null)
                    {
                        FormsAuthentication.SetAuthCookie(userName, rememberMe);
                    }

                    result.isSuccessful = true;
                }

                RenderView("Json", result);
            }
        }

        /// <summary>
        /// Logouts the currently logged in user. This is an Ajax Operation.
        /// </summary>
        [ControllerAction]
        public void Logout()
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (IsUserAuthenticated)
                {
                    //The following check is required for TDD 
                    if (HttpContext != null)
                    {
                        FormsAuthentication.SignOut();
                    }

                    result.isSuccessful = true;
                }
                else
                {
                    result.errorMessage = "You are not logged in.";
                }

                RenderView("Json", result);
            }
        }

        /// <summary>
        /// Sends the newly generated random password. This is an Ajax Operation.
        /// </summary>
        /// <param name="email">The email (Mandatory).</param>
        [ControllerAction]
        public void SendPassword(string email)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (string.IsNullOrEmpty(email))
                {
                    result.errorMessage = "Email cannot be blank.";
                }
                else if (!IsValidEmail(email))
                {
                    result.errorMessage = "Invalid email address.";
                }
                else
                {
                    string userName = UserManager.GetUserNameByEmail(email);

                    if (string.IsNullOrEmpty(userName))
                    {
                        result.errorMessage = "Did not find any user for specified email.";
                    }
                    else
                    {
                        MembershipUser user = UserManager.GetUser(userName, false);

                        string password = user.ResetPassword();

                        //Only send mail when we are not running the unit test.
                        if (HttpContext != null)
                        {
                            SendPasswordMail(user.Email, password);
                        }

                        result.isSuccessful = true;
                    }
                }

                RenderView("Json", result);
            }
        }

        /// <summary>
        /// Creates a New User. This is an Ajax Operation.
        /// </summary>
        /// <param name="userName">Name of the user (Mandatory and must be unique).</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email (Mandatory and must be unique).</param>
        [ControllerAction]
        public void Signup(string userName, string password, string email)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (string.IsNullOrEmpty(userName))
                {
                    result.errorMessage = "User name cannot be blank.";
                }
                else if (string.IsNullOrEmpty(password))
                {
                    result.errorMessage = "Password cannot be blank.";
                }
                else if (password.Length < UserManager.MinRequiredPasswordLength)
                {
                    result.errorMessage = string.Format("Password must be {0} character long.", UserManager.MinRequiredPasswordLength);
                }
                else if (string.IsNullOrEmpty(email))
                {
                    result.errorMessage = "Email cannot be blank.";
                }
                else if (!IsValidEmail(email))
                {
                    result.errorMessage = "Invalid email address.";
                }
                else
                {
                    try
                    {
                        MembershipCreateStatus status;

                        MembershipUser user = UserManager.CreateUser(userName, password, email, null, null, true, null, out status);

                        if (user == null)
                        {
                            throw new MembershipCreateUserException(status);
                        }
                        else
                        {
                            //The following check is required for TDD 
                            if (HttpContext != null)
                            {
                                FormsAuthentication.SetAuthCookie(userName, false);

                                //Only send mail when we are not running the unit test.
                                SendSignupMail(userName, password, email);
                            }

                            result.isSuccessful = true;
                        }
                    }
                    catch (MembershipCreateUserException e)
                    {
                        result.errorMessage = e.Message;
                    }
                }

                RenderView("Json", result);
            }
        }

        private void SendPasswordMail(string email, string password)
        {
            // TODO SendNewPassword mail
        }

        private void SendSignupMail(string userName, string password, string email)
        {
            // TODO Send Signup mail
        }

        private static bool IsValidEmail(string email)
        {
            return EmailExpression.IsMatch(email);
        }
    }
}