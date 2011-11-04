namespace Kigg.Domain.Entities
{
    using System;
    using System.Diagnostics;

    public static class UserExtensions
    {
        [DebuggerStepThrough]
        public static bool HasDefaultOpenIDEmail(this User user)
        {
            throw new NotImplementedException();
            //return user.IsOpenIDAccount() && (string.Compare(IoC.Resolve<IConfigurationSettings>().DefaultEmailOfOpenIdUser, user.Email, StringComparison.OrdinalIgnoreCase) == 0);
        }

        [DebuggerStepThrough]
        public static bool IsOpenIDAccount(this User user)
        {
            return string.IsNullOrEmpty(user.Password);
        }

        [DebuggerStepThrough]
        public static bool IsAdministrator(this User user)
        {
            return HasRole(user, Role.Administrator);
        }

        [DebuggerStepThrough]
        public static bool IsModerator(this User user)
        {
            return HasRole(user, Role.Moderator);
        }

        [DebuggerStepThrough]
        public static bool IsBot(this User user)
        {
            return HasRole(user, Role.Bot);
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
            throw new NotImplementedException();
            //return (user != null) && (!IsPublicUser(user) || (user.CurrentScore > IoC.Resolve<IConfigurationSettings>().MaximumUserScoreToShowCaptcha));
        }

        [DebuggerStepThrough]
        private static bool HasRole(User user, Role role)
        {
            return (user.Role & role) == role;
        }
    }
}