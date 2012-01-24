namespace Kigg.Web
{
    using System;
    using System.Web;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;

    public class Cookie : ICookie
    {
        private readonly HttpContextBase httpContext;

        private static bool defaultHttpOnly = true;
        private static float defaultExpireDurationInMinutes = 5;

        public Cookie(HttpContextBase httpContext)
        {
            Check.Argument.IsNotNull(httpContext, "httpContext");

            this.httpContext = httpContext;
        }

        public static bool DefaultHttpOnly
        {
            [DebuggerStepThrough]
            get
            {
                return defaultHttpOnly;
            }

            [DebuggerStepThrough]
            set
            {
                defaultHttpOnly = value;
            }
        }

        public static float DefaultExpireDurationInMinutes
        {
            [DebuggerStepThrough]
            get
            {
                return defaultExpireDurationInMinutes;
            }

            [DebuggerStepThrough]
            set
            {
                Check.Argument.IsNotZeroOrNegative(value, "value");

                defaultExpireDurationInMinutes = value;
            }
        }

        public T GetValue<T>(string name)
        {
            return GetValue<T>(name, true);
        }

        public T GetValue<T>(string name, bool expireOnceRead)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");

            HttpCookie cookie = httpContext.Request.Cookies[name];
            T value = default(T);

            if (cookie != null)
            {
                if (!string.IsNullOrWhiteSpace(cookie.Value))
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                    try
                    {
                        value = (T)converter.ConvertFromString(cookie.Value);
                    }
                    catch (NotSupportedException)
                    {
                        if (converter.CanConvertFrom(typeof(string)))
                        {
                            value = (T)converter.ConvertFrom(cookie.Value);
                        }
                    }
                }

                if (expireOnceRead)
                {
                    cookie = httpContext.Response.Cookies[name];

                    if (cookie != null)
                    {
                        cookie.Expires = DateTime.Now.AddMinutes(-1);
                    }
                }
            }

            return value;
        }

        public NameValueCollection GetValues(string name, bool expireOnceRead = false)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");

            NameValueCollection cookieValues = null;

            HttpCookie cookie = httpContext.Request.Cookies[name];
            if (cookie != null)
            {
                cookieValues = cookie.Values;
                if (expireOnceRead)
                {
                    cookie = new HttpCookie(name) { Expires = DateTime.Now.AddDays(-1) };
                    httpContext.Response.Cookies.Set(cookie);
                }
            }

            return cookieValues;
        }

        public void SetValue<T>(string name, T value)
        {
            SetValue(name, value, DefaultExpireDurationInMinutes, DefaultHttpOnly);
        }

        public void SetValue<T>(string name, T value, float expireDurationInMinutes)
        {
            SetValue(name, value, expireDurationInMinutes, DefaultHttpOnly);
        }

        public void SetValue<T>(string name, T value, bool httpOnly)
        {
            SetValue(name, value, DefaultExpireDurationInMinutes, httpOnly);
        }

        public void SetValue<T>(string name, T value, float expireDurationInMinutes, bool httpOnly)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name");
            Check.Argument.IsNotZeroOrNegative(expireDurationInMinutes, "expireDurationInMinutes");

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            string cookieValue = string.Empty;

            try
            {
                cookieValue = converter.ConvertToString(value);
            }
            catch (NotSupportedException)
            {
                if (converter.CanConvertTo(typeof(string)))
                {
                    cookieValue = (string)converter.ConvertTo(value, typeof(string));
                }
            }

            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                var cookie = new HttpCookie(name)
                                 {
                                     Value = cookieValue,
                                     Expires = DateTime.Now.AddMinutes(expireDurationInMinutes),
                                     HttpOnly = httpOnly
                                 };

                httpContext.Response.Cookies.Add(cookie);
            }
        }

        public void SetValue(string name, NameValueCollection values, float expireDurationInMinutes = 15, bool httpOnly = false)
        {
            Check.Argument.IsNotNull(name, "name");
            Check.Argument.IsNotNullOrEmpty(values, "values");
            Check.Argument.IsNotZeroOrNegative(expireDurationInMinutes, "expireDurationInMinutes");

            var cookie = new HttpCookie(name)
                             {
                                 Expires = DateTime.Now.AddMinutes(expireDurationInMinutes),
                                 HttpOnly = httpOnly
                             };

            cookie.Values.Add(values);

            httpContext.Response.Cookies.Set(cookie);
        }
    }
}