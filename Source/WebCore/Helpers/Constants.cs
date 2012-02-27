namespace Kigg.Web
{
    public static class Constants
    {
        public const string DefaultTheme = "default";
        public const string DefaultLogo = "logo.png";

        public static class RouteNames
        {
            public const string Root = "~/";
            public const string Default = "Default";
            public const string Published = "Published";
            public const string OpenId = "OpenId";
            public const string Login = "Login";
            public const string Logout = "Logout";
            public const string Signup = "Signup";
            public const string Xdrs = "Xdrs";
            public const string JsonConstants = "JsonConstants";
        }

        public static class CookieNames
        {
            internal const string Notification = "notification";
            public const string Msg = "msg";
            public const string Err = "err";
            internal const string AuthRememberMe = "auth_remember_me";
            internal const string AuthReturnUrl = "auth_return_rul";
            internal const string AuthForm = "auth_aspnetmvc_forms";
        }
    }
}
