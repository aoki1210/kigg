namespace Kigg
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
    using System.Web.Security;

    /// <summary>
    /// Handles all Membership related operations.
    /// </summary>
    public class UserController : BaseController
    {
        private static readonly Regex EmailExpression = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly string AdminEmail = ConfigurationManager.AppSettings["adminEmail"];

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
        [CompressFilter]
        public void Login(string userName, string password, bool rememberMe)
        {
            using (CodeBenchmark.Start)
            {
                var result = new JsonResult();

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
                    //The following try block is required for TDD 
                    try
                    {
                        FormsAuthentication.SetAuthCookie(userName, rememberMe);
                    }
                    catch(NullReferenceException)
                    {
                    }

                    result.isSuccessful = true;
                }

                RenderView("Json", result);
            }
        }

        /// <summary>
        /// Logouts the currently logged in user. This is an Ajax Operation.
        /// </summary>
        [CompressFilter]
        public void Logout()
        {
            using (CodeBenchmark.Start)
            {
                var result = new JsonResult();

                if (IsUserAuthenticated)
                {
                    //The following try block is required for TDD 
                    try
                    {
                        FormsAuthentication.SignOut();
                    }
                    catch (NullReferenceException)
                    {
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
        [CompressFilter]
        public void SendPassword(string email)
        {
            using (CodeBenchmark.Start)
            {
                var result = new JsonResult();

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
                    var userName = UserManager.GetUserNameByEmail(email);

                    if (string.IsNullOrEmpty(userName))
                    {
                        result.errorMessage = "Did not find any user for specified email.";
                    }
                    else
                    {
                        var user = UserManager.GetUser(userName, false);

                        var password = user.ResetPassword();

                        //The following try block is required for TDD 
                        try
                        {
                            SendPasswordMail(user.Email, password);
                            result.isSuccessful = true;
                        }
                        catch (NotImplementedException)
                        {
                            result.isSuccessful = true;
                        }
                        catch(Exception e)
                        {
                            result.errorMessage = e.Message;
                        }
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
        [CompressFilter]
        public void Signup(string userName, string password, string email)
        {
            using (CodeBenchmark.Start)
            {
                var result = new JsonResult();

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
                    result.errorMessage = string.Format(CultureInfo.InvariantCulture, "Password must be {0} character long.", UserManager.MinRequiredPasswordLength);
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

                        var user = UserManager.CreateUser(userName, password, email, null, null, true, null, out status);

                        if (user == null)
                        {
                            throw new MembershipCreateUserException(status);
                        }

                        //The following try block is required for TDD 
                        try
                        {
                            FormsAuthentication.SetAuthCookie(userName, false);
                        }
                        catch (NullReferenceException)
                        {
                        }

                        //Also the following try block is required for TDD 
                        try
                        {
                            //Only send mail when we are not running the unit test.
                            SendSignupMail(userName, password, email);
                            result.isSuccessful = true;
                        }
                        catch(NotImplementedException)
                        {
                            result.isSuccessful = true;
                        }
                        catch(Exception e)
                        {
                            result.errorMessage = e.Message;
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
            const string CacheKey = "passwordRecoveryMailTemplate";

            var body = HttpContext.Cache[CacheKey] as string;

            if (string.IsNullOrEmpty(body))
            {
                var file = System.Web.HttpContext.Current.Server.MapPath("~/MailTemplates/Password.txt");
                body = File.ReadAllText(file);

                HttpContext.Cache[CacheKey] = body;
            }

            body = body.Replace("<%Password%>", password);

            SendMail(email, "Kigg.com: New Password", body);
        }

        private void SendSignupMail(string userName, string password, string email)
        {
            const string CacheKey = "signupMailTemplate";

            var body = HttpContext.Cache[CacheKey] as string;

            if (body == null)
            {
                var file = System.Web.HttpContext.Current.Server.MapPath("~/MailTemplates/Signup.txt");
                body = File.ReadAllText(file);

                HttpContext.Cache[CacheKey] = body;
            }

            body = body.Replace("<%UserName%>", userName);
            body = body.Replace("<%Password%>", password);

            SendMail(email, "Kigg.com: Signup", body);
        }

        private static void SendMail(string to, string subject, string body)
        {
            using(var message = new MailMessage())
            {
                message.From = new MailAddress(AdminEmail);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                var mailClient = new SmtpClient();
                mailClient.Send(message);
            }
        }

        private static bool IsValidEmail(string email)
        {
            return EmailExpression.IsMatch(email);
        }
    }
}