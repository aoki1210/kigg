namespace Kigg.Domain.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "User Name required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid Email format.")]
        public string Email { get; set; }
    }
}
