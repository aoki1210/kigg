namespace Kigg.Web.Controllers
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.Mvc;
    using DotNetOpenAuth.Messaging;
    using DotNetOpenAuth.OpenId;
    using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
    using DotNetOpenAuth.OpenId.RelyingParty;
    using Kigg.Domain.ViewModels;
    using Kigg.Services;
    using Resources;
    using Security;
    public class MembershipController : KiggControllerBase
    {
        private const string CookieRememberMe = Constants.CookieNames.AuthRememberMe;
        private const string CookieReturnUrl = Constants.CookieNames.AuthReturnUrl;
        private const int SevenDays = 60 * 60 * 24 * 7;

        private readonly IMembershipService membershipService;
        private readonly IOpenIdRelyingParty openId;
        private readonly IFormsAuthentication formsAuthentication;
        private readonly ICookie cookie;

        public MembershipController(IMembershipService membershipService ,IOpenIdRelyingParty openId, IFormsAuthentication formsAuthentication, ICookie cookie)
        {
            Check.Argument.IsNotNull(membershipService, "openId");
            Check.Argument.IsNotNull(openId, "openId");
            Check.Argument.IsNotNull(formsAuthentication, "formsAuthentication");
            Check.Argument.IsNotNull(cookie, "cookie");

            this.membershipService = membershipService;
            this.openId = openId;
            this.formsAuthentication = formsAuthentication;
            this.cookie = cookie;
        }

        [OutputCache(Duration = SevenDays, VaryByParam = "none")]
        public ActionResult Xrds()
        {
            const string xrds = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<xrds:XRDS xmlns:xrds=\"xri://$xrds\" xmlns:openid=\"http://openid.net/xmlns/1.0\" xmlns=\"xri://$xrd*($v*2.0)\">" +
                                    "<XRD>" +
                                        "<Service priority=\"1\">" +
                                            "<Type>http://specs.openid.net/auth/2.0/return_to</Type>" +
                                            "<URI>{0}</URI>" +
                                        "</Service>" +
                                    "</XRD>" +
                                "</xrds:XRDS>";

            string url = Url.ToAbsolute(Url.OpenId());

            string xml = xrds.FormatWith(url);

            return Content(xml, "application/xrds+xml");
        }

        [AppendOpenIdXrdsLocation]
        public ActionResult OpenId()
        {
            IAuthenticationResponse response = openId.Response;

            if (response != null)
            {
                if ((response.Status == AuthenticationStatus.Failed) || (response.Status == AuthenticationStatus.Canceled))
                {
                    SetErrorCookie(TextMessages.UnableToLoginWithYourPreferredOpenIDProvider);

                    //ModelState.AddModelError(ModelStateUserNameKey, TextMessages.UnableToLoginWithYourPreferredOpenIDProvider);
                }
                else if (response.Status == AuthenticationStatus.Authenticated)
                {
                    string userName = response.ClaimedIdentifier;
                    var fetch = response.GetExtension<ClaimsResponse>();

                    // Some of the Provider does not return Email
                    // Such as Yahoo, Blogger, Bloglines etc, in that case email will be null
                    string email = (fetch != null) ? fetch.Email : null;

                    //UserResult result = userService.Save(userName, email);
                    //ModelState.Merge(result.RuleViolations);

                    if (ModelState.IsValid)
                    {
                        var persistCookie = cookie.GetValue<bool>(CookieRememberMe);
                        var returnUrl = cookie.GetValue<string>(CookieReturnUrl);

                        formsAuthentication.SetAuthenticationCookie(userName, persistCookie);

                        return Redirect(returnUrl ?? Url.Home());
                    }
                }
            }
            return RedirectToRoute(Constants.RouteNames.Published);
        }

        [HttpPost]
        public ActionResult OpenId(OpenIdCommand command)
        {
            Check.Argument.IsNotNull(command, "command");

            Identifier id;
            bool failed = false;
            if (string.IsNullOrWhiteSpace(command.UserName) || !Identifier.TryParse(command.UserName, out id))
            {
                string errorMessage = string.IsNullOrWhiteSpace(command.UserName) ? TextMessages.OpenIDUserNameCannotBeBlank : TextMessages.InvalidOpenIDUserName;

                SetErrorCookie(errorMessage);
                failed = true;
            }
            else
            {
                cookie.SetValue(CookieRememberMe, command.RememberMe ?? false);
                cookie.SetValue(CookieReturnUrl, !string.IsNullOrWhiteSpace(command.ReturnUrl) ? command.ReturnUrl : Url.Home());

                try
                {
                    var realm = new Realm(new Uri(Url.ToAbsolute(Url.OpenId())));
                    IAuthenticationRequest request = openId.CreateRequest(id, realm);

                    var fetch = new ClaimsRequest { Email = DemandLevel.Request };
                    request.AddExtension(fetch);

                    request.RedirectToProvider();
                }
                catch (ProtocolException e)
                {
                    SetErrorCookie(e.Message);
                    failed = true;
                }
            }

            return !failed
                       ? new EmptyResult()
                       : (ActionResult)RedirectToRoute(Constants.RouteNames.Default);
        }

        [HttpPost]
        public ActionResult Logout(string returnUrl)
        {
            formsAuthentication.Logout();

            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? Url.Home() : returnUrl);
        }

        [HttpPost]
        public ActionResult Signup(UserRegistrationModel userRegistration)
        {
            membershipService.CreateUser(userRegistration);
            //TODO: Perform Signup
            return Redirect(Url.Home());
        }

        private void SetErrorCookie(string message)
        {
            var values = new NameValueCollection
                                     {
                                         {Constants.CookieNames.Msg, message},
                                         {Constants.CookieNames.Err, true.ToString(CultureInfo.InvariantCulture)},
                                     };
            cookie.SetValue(Constants.CookieNames.Notification, values, 2, false);
        }
    }


}
