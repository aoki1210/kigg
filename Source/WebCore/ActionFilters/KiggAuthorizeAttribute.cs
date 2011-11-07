namespace Kigg.Web.Security
{
    using System;
    //using System.Security.Principal;
    using System.Web.Mvc;

    using MvcExtensions;

    using Domain.Entities;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false), CLSCompliant(false)]
    public class KiggAuthorizeAttribute : ExtendedAuthorizeAttribute
    {
        public Role? AllowedRole
        {
            get;
            set;
        }

        public override bool IsAuthorized(AuthorizationContext filterContext)
        {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            //TODO: Real implementation here
            return true;
        }
    }
}
