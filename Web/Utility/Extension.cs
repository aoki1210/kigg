namespace Kigg
{
    using System;
    using System.Text;

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
}