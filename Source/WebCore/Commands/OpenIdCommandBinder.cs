namespace Kigg.Web.Security
{
    using System.Web.Mvc;

    using MvcExtensions;

    public class OpenIdCommandBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Invariant.IsNotNull(bindingContext, "bindingContext");

            var command = (OpenIdCommand)base.BindModel(controllerContext, bindingContext);

            if (string.IsNullOrWhiteSpace(command.UserName))
            {
                ValueProviderResult result = bindingContext.ValueProvider.GetValue("openid_identifier") ??
                                             bindingContext.ValueProvider.GetValue("openid_username");

                if (result != null)
                {
                    command.UserName = result.AttemptedValue;
                }
            }

            if (!command.RememberMe.HasValue)
            {
                ValueProviderResult result = bindingContext.ValueProvider.GetValue("openid_identifier_remember") ??
                                             bindingContext.ValueProvider.GetValue("openid_username_remember");

                if (result != null)
                {
                    command.RememberMe = (bool?)result.ConvertTo(typeof(bool?), Culture.Current);
                }
            }

            return command;
        }
    }
}