namespace Kigg.Domain.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class UserRegistrationModel
    {
        private const string EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        [Required(ErrorMessageResourceType = typeof(Resources.TextMessages), ErrorMessageResourceName = Strings.ValidationResourceKeys.ValidationUserNameRequired)]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.TextMessages), ErrorMessageResourceName = Strings.ValidationResourceKeys.ValidationPasswordRequired)]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.TextMessages), ErrorMessageResourceName = Strings.ValidationResourceKeys.ValidationEmailRequired)]
        [RegularExpression(EmailPattern, ErrorMessageResourceType = typeof(Resources.TextMessages), ErrorMessageResourceName = Strings.ValidationResourceKeys.ValidationEmailInvalidFormat)]
        public string Email { get; set; }
    }
}
