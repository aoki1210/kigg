namespace Kigg
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web.Security;
    using System.Web.Mvc;

    public class UserController : BaseController
    {
        private static readonly Regex EmailExpression = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled | RegexOptions.Singleline);

        public UserController(MembershipProvider userManager): base(userManager)
        {
        }

        public UserController():base()
        {
        }

        [ControllerAction()]
        public void Login(string userName, string password, bool rememberMe)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (string.IsNullOrEmpty(userName))
                {
                    result.errorMessage = "User name cannot be blank.";
                }
                else
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        result.errorMessage = "Password cannot be blank.";
                    }
                    else
                    {
                        try
                        {
                            if (UserManager.ValidateUser(userName, password))
                            {
                                //The following check is required for TDD 
                                if (HttpContext != null)
                                {
                                    FormsAuthentication.SetAuthCookie(userName, rememberMe);
                                }

                                result.isSuccessful = true;
                            }
                            else
                            {
                                result.errorMessage = "Invalid login credentials.";
                            }
                        }
                        catch (Exception e)
                        {
                            result.errorMessage = e.Message;
                        }
                    }
                }

                RenderView("Json", result);
            }
        }

        [ControllerAction()]
        public void Logout()
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (IsUserAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    result.isSuccessful = true;
                }
                else
                {
                    result.errorMessage = "You are not logged in";
                }

                RenderView("Json", result);
            }
        }

        [ControllerAction()]
        public void SendPassword(string email)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (string.IsNullOrEmpty(email))
                {
                    result.errorMessage = "Email cannot be blank.";
                }
                else
                {
                    if (!IsValidEmail(email))
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

                            SendPasswordMail(user.Email, password);

                            result.isSuccessful = true;
                        }
                    }
                }

                RenderView("Json", result);
            }
        }

        [ControllerAction()]
        public void Signup(string userName, string password, string email)
        {
            using (new CodeBenchmark())
            {
                JsonResult result = new JsonResult();

                if (string.IsNullOrEmpty(userName))
                {
                    result.errorMessage = "User name cannot be blank.";
                }
                else
                {
                    if (string.IsNullOrEmpty(password))
                    {
                        result.errorMessage = "Password cannot be blank.";
                    }
                    else
                    {
                        if (password.Length < UserManager.MinRequiredPasswordLength)
                        {
                            result.errorMessage = string.Format("Password must be {0} character long.", UserManager.MinRequiredPasswordLength);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(email))
                            {
                                result.errorMessage = "Email cannot be blank.";
                            }
                            else
                            {
                                if (!IsValidEmail(email))
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
                                            }

                                            SendSignupMail(userName, password, email);

                                            result.isSuccessful = true;
                                        }
                                    }
                                    catch (MembershipCreateUserException e)
                                    {
                                        result.errorMessage = e.Message;
                                    }
                                }
                            }
                        }
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