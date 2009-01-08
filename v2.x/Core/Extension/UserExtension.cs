namespace Kigg.DomainObjects
{
    using System;

    using Infrastructure;

    public static class UserExtension
    {
        public static bool HasDefaultOpenIDEmail(this IUser user)
        {
            return user.IsOpenIDAccount() && (string.Compare(IoC.Resolve<IConfigurationSettings>().DefaultEmailOfOpenIdUser, user.Email, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public static bool IsOpenIDAccount(this IUser user)
        {
            return string.IsNullOrEmpty(user.Password);
        }

        public static bool IsAdministrator(this IUser user)
        {
            return HasRole(user, Roles.Administrator);
        }

        public static bool IsModerator(this IUser user)
        {
            return HasRole(user, Roles.Moderator);
        }

        public static bool IsBot(this IUser user)
        {
            return HasRole(user, Roles.Bot);
        }

        public static bool IsPublicUser(this IUser user)
        {
            return !IsBot(user) && !IsModerator(user) && !IsAdministrator(user);
        }

        public static bool CanModerate(this IUser user)
        {
            return IsAdministrator(user) || IsModerator(user);
        }

        private static bool HasRole(IUser user, Roles role)
        {
            return (user.Role & role) == role;
        }
    }
}