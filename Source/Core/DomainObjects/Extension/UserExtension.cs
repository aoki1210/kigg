namespace Kigg.DomainObjects
{
    using System;
    using System.Diagnostics;

    using Infrastructure;

    public static class UserExtension
    {
        [DebuggerStepThrough]
        public static bool HasDefaultOpenIDEmail(this User user)
        {
            return user.IsOpenIDAccount() && (string.Compare(IoC.Resolve<IConfigurationSettings>().DefaultEmailOfOpenIdUser, user.Email, StringComparison.OrdinalIgnoreCase) == 0);
        }

        [DebuggerStepThrough]
        public static bool IsOpenIDAccount(this User user)
        {
            return string.IsNullOrEmpty(user.Password);
        }

        [DebuggerStepThrough]
        public static bool IsAdministrator(this User user)
        {
            return HasRole(user, Roles.Administrator);
        }

        [DebuggerStepThrough]
        public static bool IsModerator(this User user)
        {
            return HasRole(user, Roles.Moderator);
        }

        [DebuggerStepThrough]
        public static bool IsBot(this User user)
        {
            return HasRole(user, Roles.Bot);
        }

        [DebuggerStepThrough]
        public static bool IsPublicUser(this User user)
        {
            return !IsBot(user) && !IsModerator(user) && !IsAdministrator(user);
        }

        [DebuggerStepThrough]
        public static bool CanModerate(this User user)
        {
            return IsAdministrator(user) || IsModerator(user);
        }

        [DebuggerStepThrough]
        public static bool ShouldHideCaptcha(this User user)
        {
            return (user != null) && (!IsPublicUser(user) || (user.CurrentScore > IoC.Resolve<IConfigurationSettings>().MaximumUserScoreToShowCaptcha));
        }

        [DebuggerStepThrough]
        private static bool HasRole(User user, Roles role)
        {
            return (user.Role & role) == role;
        }
    }
}