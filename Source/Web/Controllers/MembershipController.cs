namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
    using DotNetOpenAuth.OpenId.RelyingParty;
    using DotNetOpenAuth.Messaging;

    using DomainObjects;
    using Infrastructure;
    using Service;

    public class MembershipController : BaseController
    {
        private const int MinimumLength = 4;

        private static readonly Regex UserNameExpression = new Regex(@"^([a-zA-Z])[a-zA-Z_-]*[\w_-]*[\S]$|^([a-zA-Z])[0-9_-]*[\S]$|^[a-zA-Z]*[\S]$", RegexOptions.Singleline | RegexOptions.Compiled);

        private readonly IDomainObjectFactory _factory;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEmailSender _emailSender;
        private readonly IBlockedIPCollection _blockedIPList;

        public MembershipController(IDomainObjectFactory factory, IEventAggregator eventAggregator, IEmailSender emailSender, IBlockedIPCollection blockedIPList)
        {
            Check.Argument.IsNotNull(factory, "factory");
            Check.Argument.IsNotNull(eventAggregator, "eventAggregator");
            Check.Argument.IsNotNull(emailSender, "emailSender");
            Check.Argument.IsNotNull(blockedIPList, "blockedIPList");

            _factory = factory;
            _eventAggregator = eventAggregator;
            _emailSender = emailSender;
            _blockedIPList = blockedIPList;
        }

        private bool OpenIdRememberMe
        {
            get
            {
                bool rememberMe = false;

                HttpCookie cookie = Request.Cookies["oidr"];

                if (cookie != null)
                {
                    if (!bool.TryParse(cookie.Value, out rememberMe))
                    {
                        rememberMe = false;
                    }

                    cookie = Response.Cookies["oidr"];

                    if (cookie != null)
                    {
                        cookie.Expire();
                    }
                }

                return rememberMe;
            }
            set
            {
                if (value)
                {
                    var cookie = new HttpCookie("oidr")
                                            {
                                                Expires = DateTime.Now.AddMinutes(5),
                                                HttpOnly = false,
                                                Value = bool.TrueString
                                            };

                    HttpContext.Response.Cookies.Add(cookie);
                }
            }
        }

        private string ReturnUrl
        {
            get
            {
                string returnUrl = string.Empty;

                HttpCookie cookie = Request.Cookies["returnUrl"];

                if (cookie != null)
                {
                    returnUrl = cookie.Value;

                    cookie = Response.Cookies["returnUrl"];

                    if (cookie != null)
                    {
                        cookie.Expire();
                    }
                }

                return returnUrl;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var cookie = new HttpCookie("returnUrl")
                                            {
                                                Expires = DateTime.Now.AddMinutes(5),
                                                HttpOnly = false,
                                                Value = value
                                            };

                    HttpContext.Response.Cookies.Add(cookie);
                }
            }
        }

        public IOpenIdRelyingParty OpenIdRelyingParty
        {
            get;
            set;
        }

        [Compress]
        public ActionResult OpenId(string identifier, bool? rememberMe)
        {
            string errorMessage = null;

            string url = string.Concat(Settings.RootUrl, Url.Content("~/xrds.axd"));
            HttpContext.Response.Headers.Add("X-XRDS-Location", url);

            try
            {
                if (OpenIdRelyingParty.Response == null)
                {
                    if (!string.IsNullOrEmpty(identifier))
                    {
                        Identifier id;

                        if (Identifier.TryParse(identifier, out id))
                        {
                            var realm = new Realm(new Uri(string.Concat(Settings.RootUrl, Url.RouteUrl("OpenId"))));

                            IAuthenticationRequest request = OpenIdRelyingParty.CreateRequest(identifier, realm);

                            var fetch = new ClaimsRequest
                                            {
                                                Email = DemandLevel.Require
                                            };

                            request.AddExtension(fetch);

                            OpenIdRememberMe = rememberMe ?? false;
                            ReturnUrl = (HttpContext.Request.UrlReferrer != null) ? HttpContext.Request.UrlReferrer.ToString() : string.Empty;

                            return request.RedirectingResponse.AsActionResult(); //.RedirectToProvider();
                        }
                    }

                    return new EmptyResult();
                }

                if (OpenIdRelyingParty.Response.Status == AuthenticationStatus.Authenticated)
                {
                    string userName = OpenIdRelyingParty.Response.ClaimedIdentifier;

                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IUser user = UserRepository.FindByUserName(userName);

                        if ((user != null) && user.IsLockedOut)
                        {
                            errorMessage = "Your account is currently locked out. Please contact the support for this issue.";
                        }
                        else
                        {
                            var fetch = OpenIdRelyingParty.Response.GetExtension<ClaimsResponse>();

                            // Some of the Provider does not return Email
                            // Such as Yahoo, Blogger, Bloglines etc, in that case we will assign a default
                            // email
                            string email = ((fetch == null) || string.IsNullOrEmpty(fetch.Email)) ? Settings.DefaultEmailOfOpenIdUser : fetch.Email;

                            if (user == null)
                            {
                                user = _factory.CreateUser(userName, email, null);
                                UserRepository.Add(user);
                                _eventAggregator.GetEvent<UserActivateEvent>().Publish(new UserActivateEventArgs(user));
                            }
                            else
                            {
                                //Sync the email from OpenID provider.
                                if ((string.Compare(email, user.Email, StringComparison.OrdinalIgnoreCase) != 0) &&
                                    (string.Compare(email, Settings.DefaultEmailOfOpenIdUser, StringComparison.OrdinalIgnoreCase) != 0))
                                {
                                    user.ChangeEmail(email);
                                }
                            }

                            user.LastActivityAt = SystemTime.Now();

                            unitOfWork.Commit();
                            FormsAuthentication.SetAuthCookie(userName, OpenIdRememberMe);
                        }
                    }
                }
                else if ((OpenIdRelyingParty.Response.Status == AuthenticationStatus.Failed) || (OpenIdRelyingParty.Response.Status == AuthenticationStatus.Canceled))
                {
                    if (OpenIdRelyingParty.Response.Exception!= null)
                    {
                        errorMessage = OpenIdRelyingParty.Response.Exception.Message;
                    }
                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "Failed to login with your preferred OpenID provider.";
                    }
                }
            }
            catch (Exception oid)
            {
                errorMessage = oid.Message;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                GenerateMessageCookie(errorMessage, true);
            }

            string returnUrl = ReturnUrl;

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToRoute("Published");
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Signup(string userName, string password, string email)
        {
            var viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(userName.NullSafe()), "User name cannot be blank."),
                                                                new Validation(() => userName.Trim().Length < MinimumLength, "User name cannot be less than {0} character.".FormatWith(MinimumLength)),
                                                                new Validation(() => !UserNameExpression.IsMatch(userName), "User name must be alphanumeric characters which starts with alphabet and can only contains special characters dash and underscore."),
                                                                new Validation(() => string.IsNullOrEmpty(password.NullSafe()), "Password cannot be blank."),
                                                                new Validation(() => password.Trim().Length < MinimumLength, "Password cannot be less than {0} character.".FormatWith(MinimumLength)),
                                                                new Validation(() => string.IsNullOrEmpty(email), "Email cannot be blank."),
                                                                new Validation(() => !email.NullSafe().IsEmail(), "Invalid email address format.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IUser user = _factory.CreateUser(userName.Trim(), email.Trim(), password.Trim());
                        UserRepository.Add(user);

                        unitOfWork.Commit();

                        string userId = user.Id.Shrink();

                        string url = string.Concat(Settings.RootUrl, Url.RouteUrl("Activate", new { id = userId }));

                        _emailSender.SendRegistrationInfo(email, userName, password, url);

                        Log.Info("User registered: {0}", user.UserName);

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (ArgumentException argument)
                {
                    viewData = new JsonViewData { errorMessage = argument.Message };
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("signing up") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Login(string userName, string password, bool? rememberMe)
        {
            var viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(userName.NullSafe()), "User name cannot be blank."),
                                                                new Validation(() => string.IsNullOrEmpty(password.NullSafe()), "Password cannot be blank.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IUser user = UserRepository.FindByUserName(userName.Trim());

                        if (user != null)
                        {
                            viewData = Validate<JsonViewData>(
                                                                new Validation(() => user.IsLockedOut, "Your account is currently locked out. Please contact the support for this issue."),
                                                                new Validation(() => !user.IsActive, "Your account is not activated yet. Please click the activation link in the registration mail to activate your account."),
                                                                new Validation(() => user.IsOpenIDAccount(), "Specified user login is only valid for OpenID.")
                                                             );

                            if (viewData == null)
                            {
                                if (string.Compare(user.Password, password.Trim().Hash(), StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    user.LastActivityAt = SystemTime.Now();
                                    unitOfWork.Commit();

                                    FormsAuthentication.SetAuthCookie(userName, rememberMe ?? false);
                                    viewData = new JsonViewData { isSuccessful = true };

                                    Log.Info("User logged in: {0}", user.UserName);
                                }
                            }
                        }

                        if (viewData == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Invalid login credentials." };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("logging in") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Logout()
        {
            JsonViewData viewData;

            if (IsCurrentUserAuthenticated)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        CurrentUser.LastActivityAt = SystemTime.Now();
                        unitOfWork.Commit();

                        FormsAuthentication.SignOut();
                        Log.Info("User logged out: {0}", CurrentUserName);
                        viewData = new JsonViewData { isSuccessful = true  };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("logging out") };
                }
            }
            else
            {
                viewData = new JsonViewData { errorMessage = "You are currently not logged in." };
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult ForgotPassword(string email)
        {
            var viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(email.NullSafe()), "Email cannot be blank."),
                                                                new Validation(() => !email.NullSafe().IsEmail(), "Invalid email address format.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IUser user = UserRepository.FindByEmail(email.Trim());

                        if (user == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Did not find any user with the specified email." };
                        }
                        else
                        {
                            try
                            {
                                string password = user.ResetPassword();

                                unitOfWork.Commit();

                                _emailSender.SendNewPassword(user.Email, user.UserName, password);

                                viewData = new JsonViewData { isSuccessful = true };

                                Log.Info("New password generated for: {0}", user.UserName);
                            }
                            catch (InvalidOperationException invalidOperation)
                            {
                                viewData = new JsonViewData { errorMessage = invalidOperation.Message };
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("resetting password") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
             var viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(oldPassword.NullSafe()), "Old password cannot be blank."),
                                                                new Validation(() => string.IsNullOrEmpty(newPassword.NullSafe()), "New password cannot be blank."),
                                                                new Validation(() => newPassword.Trim().Length < MinimumLength, "New password cannot be less than {0} character.".FormatWith(MinimumLength)),
                                                                new Validation(() => string.CompareOrdinal(newPassword.NullSafe(), confirmPassword.NullSafe()) != 0, "Confirm password does not match with the new password."),
                                                                new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        CurrentUser.ChangePassword(oldPassword.Trim(), newPassword.Trim());
                        unitOfWork.Commit();

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (InvalidOperationException invalidOperation)
                {
                    viewData = new JsonViewData { errorMessage = invalidOperation.Message };
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("changing password") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult ChangeEmail(string email)
        {
            var viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(email.NullSafe()), "Email cannot be blank."),
                                                                new Validation(() => !email.NullSafe().IsEmail(), "Invalid email address format."),
                                                                new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        CurrentUser.ChangeEmail(email.Trim());
                        unitOfWork.Commit();

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (InvalidOperationException invalidOperation)
                {
                    viewData = new JsonViewData { errorMessage = invalidOperation.Message };
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("changing email") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult ChangeRole(string id, string role)
        {
            var viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(id), "User Id cannot be blank."),
                                                                new Validation(() => string.IsNullOrEmpty(role), "Role cannot be blank."),
                                                                new Validation(() => id.ToGuid().IsEmpty(), "Invalid user identifier."),
                                                                new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated."),
                                                                new Validation(() => !CurrentUser.IsAdministrator(), "You do not have the privilege to call this method.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IUser user = UserRepository.FindById(id.ToGuid());

                        if (user == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Specified user no longer exist." };
                        }
                        else
                        {
                            user.Role = role.ToEnum(user.Role);
                            unitOfWork.Commit();

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("changing role") };
                }
            }

            return Json(viewData);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Lock(string id)
        {
            return LockOrUnlock(id, false);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult Unlock(string id)
        {
            return LockOrUnlock(id, true);
        }

        [AcceptVerbs(HttpVerbs.Post), Compress]
        public ActionResult AllowIps(string id, ICollection<string> ipAddress)
        {
            var viewData = Validate<JsonViewData>(
                                                                new Validation(() => string.IsNullOrEmpty(id), "User Id cannot be blank."),
                                                                new Validation(() => id.ToGuid().IsEmpty(), "Invalid user identifier."),
                                                                new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated."),
                                                                new Validation(() => !CurrentUser.IsAdministrator(), "You do not have the privilege to call this method.")
                                                          );

            if (viewData == null)
            {
                try
                {
                    IUser user = UserRepository.FindById(id.ToGuid());

                    if (user == null)
                    {
                        viewData = new JsonViewData { errorMessage = "Specified user no longer exist." };
                    }
                    else
                    {
                        ICollection<string> usedIpAddresses = UserRepository.FindIPAddresses(user.Id);
                        var blockedIps = new List<string>();

                        ipAddress = ipAddress ?? new List<string>();

                        if (!usedIpAddresses.IsNullOrEmpty())
                        {
                            foreach (string usedIpAddress in usedIpAddresses)
                            {
                                if (!ipAddress.Contains(usedIpAddress))
                                {
                                    blockedIps.Add(usedIpAddress);
                                }
                            }
                        }

                        if (blockedIps.Count > 0)
                        {
                            _blockedIPList.AddRange(blockedIps);
                        }

                        if (ipAddress.Count > 0)
                        {
                            _blockedIPList.RemoveRange(ipAddress);
                        }

                        viewData = new JsonViewData { isSuccessful = true };
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("allowing the user Ip Addresses") };
                }
            }

            return Json(viewData);
        }

        [Compress]
        public ActionResult Activate(string id)
        {
            Guid userId = id.NullSafe().ToGuid();

            if (!userId.IsEmpty())
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IUser user = UserRepository.FindById(userId);

                        if ((user != null) && !user.IsActive)
                        {
                            user.IsActive = true;
                            user.LastActivityAt = SystemTime.Now();

                            _eventAggregator.GetEvent<UserActivateEvent>().Publish(new UserActivateEventArgs(user));

                            unitOfWork.Commit();

                            Log.Info("Account activated for: {0}", user.UserName);

                            GenerateMessageCookie("Your account is activated. You can log in now.", false);
                        }
                        else
                        {
                            GenerateMessageCookie("Invalid activation key.", true);
                        }
                    }
                }
                catch (Exception e)
                {
                    GenerateMessageCookie(FormatStrings.UnknownError.FormatWith("activating your account"), true);
                    Log.Exception(e);
                }
            }

            return RedirectToRoute("Published");
        }

        [Compress]
        public ActionResult List(int? page)
        {
            var viewData = CreateViewData<UserListViewData>();

            PagedResult<IUser> pagedResult = UserRepository.FindAll(PageCalculator.StartIndex(page, Settings.HtmlUserPerPage), Settings.HtmlUserPerPage);

            viewData.CurrentPage = page ?? 1;
            viewData.UserPerPage = Settings.HtmlUserPerPage;
            viewData.Users = pagedResult.Result;
            viewData.TotalUserCount = pagedResult.Total;

            viewData.Title = "{0} - Users".FormatWith(Settings.SiteTitle);
            viewData.Subtitle = "Users";
            viewData.NoUserExistMessage = "No user exists.";

            return View(viewData);
        }

        [Compress]
        public ActionResult Detail(string name, string tab, int? page)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToRoute("Users", new { page = 1});
            }

            IUser user = null;
            Guid userId = name.NullSafe().ToGuid();

            if (!userId.IsEmpty())
            {
                user = UserRepository.FindById(userId);
            }

            if (user == null)
            {
                ThrowNotFound("User no longer exists.");
            }

            UserDetailTab selectedTab = tab.ToEnum(UserDetailTab.Promoted);
            var viewData = CreateViewData<UserDetailViewData>();

            viewData.CurrentPage = page ?? 1;
            viewData.SelectedTab = selectedTab;
            viewData.TheUser = user;
            viewData.IPAddresses = new Dictionary<string, bool>();

            if ((user != null) && IsCurrentUserAuthenticated && CurrentUser.IsAdministrator())
            {
                foreach (string ipAddress in UserRepository.FindIPAddresses(user.Id))
                {
                    bool isAllowed = !_blockedIPList.Contains(ipAddress);

                    viewData.IPAddresses.Add(ipAddress, isAllowed);
                }
            }

            return View(viewData);
        }

        [ChildActionOnly]
        [ValidateInput(false)]
        public ActionResult Menu()
        {
            return View(new UserMenuViewData { IsUserAuthenticated = IsCurrentUserAuthenticated, CurrentUser = CurrentUser });
        }

        [ChildActionOnly]
        public ActionResult TopTabs()
        {
            DateTime maxTimestamp = SystemTime.Now();

            ICollection<UserWithScore> topLeaders = UserRepository.FindTop(Constants.ProductionDate, maxTimestamp, 0, Settings.TopUsers)
                                                                  .Result.Select(u => new UserWithScore { User = u, Score = u.CurrentScore })
                                                                  .ToList()
                                                                  .AsReadOnly();

            DateTime minTimestamp = maxTimestamp.AddDays(-1);

            ICollection<UserWithScore> topMovers = UserRepository.FindTop(minTimestamp, maxTimestamp, 0, Settings.TopUsers)
                                                                 .Result.Select(u => new UserWithScore { User = u, Score = u.GetScoreBetween(minTimestamp, maxTimestamp) })
                                                                 .ToList()
                                                                 .AsReadOnly();

            var viewData = new TopUserTabsViewData
                               {
                                   TopLeaders = topLeaders,
                                   TopMovers = topMovers
                               };

            return View(viewData);
        }

        private ActionResult LockOrUnlock(string id, bool unlock)
        {
            var viewData = Validate<JsonViewData>(
                new Validation(() => string.IsNullOrEmpty(id), "User Id cannot be blank."),
                new Validation(() => id.ToGuid().IsEmpty(), "Invalid user identifier."),
                new Validation(() => !IsCurrentUserAuthenticated, "You are currently not authenticated."),
                new Validation(() => !CurrentUser.IsAdministrator(),
                               "You do not have the privilege to call this method.")
                );

            if (viewData == null)
            {
                try
                {
                    using (IUnitOfWork unitOfWork = UnitOfWork.Begin())
                    {
                        IUser user = UserRepository.FindById(id.ToGuid());

                        if (user == null)
                        {
                            viewData = new JsonViewData { errorMessage = "Specified user no longer exist." };
                        }
                        else
                        {
                            if (unlock)
                            {
                                user.Unlock();
                            }
                            else
                            {
                                user.Lock();
                            }

                            unitOfWork.Commit();

                            viewData = new JsonViewData { isSuccessful = true };
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Exception(e);

                    viewData = new JsonViewData { errorMessage = FormatStrings.UnknownError.FormatWith("{0} the user".FormatWith(unlock ? "unlocking" : "locking")) };
                }
            }

            return Json(viewData);
        }

        private void GenerateMessageCookie(string message, bool isError)
        {
            var cookie = new HttpCookie("notification")
                             {
                                 Expires = DateTime.Now.AddMinutes(5),
                                 HttpOnly = false
                             };

            cookie.Values.Add("msg", message);
            cookie.Values.Add("err", isError.ToString());

            HttpContext.Response.Cookies.Add(cookie);
        }
    }
}