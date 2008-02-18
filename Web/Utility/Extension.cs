namespace Kigg
{
    using System;
    using System.Text;
    using System.Web;

    public static class DataTimeExtension
    {
        public static string Ago(this DateTime target)
        {
            StringBuilder result = new StringBuilder();
            TimeSpan diff = (DateTime.Now - target.ToLocalTime());

            if (diff.Days > 0)
            {
                result.AppendFormat("{0} days", diff.Days);
            }

            if (diff.Hours > 0)
            {
                if (result.Length > 0)
                {
                    result.Append(", ");
                }

                result.AppendFormat("{0} hours", diff.Hours);
            }

            if (diff.Minutes > 0)
            {
                if (result.Length > 0)
                {
                    result.Append(", ");
                }

                result.AppendFormat("{0} minutes", diff.Minutes);
            }

            if (result.Length == 0)
            {
                result.Append("few moments");
            }

            return result.ToString();
        }
    }

    public static class StringExtension
    {
        public static string UrlEncode(this string target)
        {
            if (target.IndexOf(" ", StringComparison.Ordinal) > -1)
            {
                target = target.Replace(" ", "_");
            }

            if (target.IndexOf("#", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("#", "sharp");
            }

            if (target.IndexOf("&", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("&", "amp");
            }

            if (target.IndexOf(".", StringComparison.Ordinal) > -1)
            {
                target = target.Replace(".", "dot");
            }

            if (target.IndexOf("/", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("/", "fws");
            }

            if (target.IndexOf("\\", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("\\", "bks");
            }

            target = HttpUtility.UrlEncode(target);

            return target;
        }

        public static string UrlDecode(this string target)
        {
            target = HttpUtility.UrlDecode(target);

            if (target.IndexOf("_", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("_", " ");
            }

            if (target.IndexOf("sharp", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("sharp", "#");
            }

            if (target.IndexOf("amp", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("amp", "&");
            }

            if (target.IndexOf("dot", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("dot", ".");
            }

            if (target.IndexOf("fws", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("fws", "/");
            }

            if (target.IndexOf("bks", StringComparison.Ordinal) > -1)
            {
                target = target.Replace("bks", "\\");
            }

            return target;
        }
    }
}