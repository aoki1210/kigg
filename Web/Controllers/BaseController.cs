namespace Kigg
{
    using System;
    using System.Diagnostics;
    using System.Web.Security;
    using System.Web.Mvc;

    public abstract class BaseController : Controller
    {
        private MembershipProvider _userManager;

        protected MembershipProvider UserManager
        {
            [DebuggerStepThrough()]
            get
            {
                return _userManager;
            }
        }

        protected bool IsUserAuthenticated
        {
            [DebuggerStepThrough()]
            get
            {
                return HttpContext.User.Identity.IsAuthenticated;
            }
        }

        protected string CurrentUserName
        {
            [DebuggerStepThrough()]
            get
            {
                return IsUserAuthenticated ? HttpContext.User.Identity.Name : "Anonymous";
            }
        }

        protected Guid CurrentUserId
        {
            [DebuggerStepThrough()]
            get
            {
                if (!IsUserAuthenticated)
                {
                    return Guid.Empty;
                }

                MembershipUser user = UserManager.GetUser(CurrentUserName, true);

                return (Guid) user.ProviderUserKey;
            }
        }

        protected BaseController(MembershipProvider userManager)
        {
            _userManager = userManager;
        }

        protected BaseController() : this(Membership.Provider)
        {
        }
    }
}