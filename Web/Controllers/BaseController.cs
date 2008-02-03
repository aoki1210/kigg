namespace Kigg
{
    using System;
    using System.Diagnostics;
    using System.Web.Security;
    using System.Web.Mvc;

    /// <summary>
    /// Base class which contains common stuffs.
    /// </summary>
    public abstract class BaseController : Controller
    {
        private readonly MembershipProvider _userManager;

        /// <summary>
        /// Gets the MembershipProvider.
        /// </summary>
        /// <value>The user manager.</value>
        protected MembershipProvider UserManager
        {
            [DebuggerStepThrough()]
            get
            {
                return _userManager;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the currently visiting user is authenticated.
        /// </summary>
        /// <value>
        /// if user authenticated returns <c>true</c>; otherwise, <c>false</c>.
        /// </value>
        protected bool IsUserAuthenticated
        {
            [DebuggerStepThrough()]
            get
            {
                return HttpContext.User.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets the name of the current user. If the user is not authenticated it returns "Anonymous"
        /// </summary>
        /// <value>The name of the current user.</value>
        protected string CurrentUserName
        {
            [DebuggerStepThrough()]
            get
            {
                return IsUserAuthenticated ? HttpContext.User.Identity.Name : "Anonymous";
            }
        }

        /// <summary>
        /// Gets the current user id. if the user is not authenticated it returns empty guid.
        /// </summary>
        /// <value>The current user id.</value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="userManager">The membership provider.</param>
        protected BaseController(MembershipProvider userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        protected BaseController() : this(Membership.Provider)
        {
        }
    }
}