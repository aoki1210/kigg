namespace Kigg.Web
{
    using System;
    using System.Text;

    public static class DateTimeExtension
    {
        public static string ToRelative(this DateTime target)
        {
            Check.Argument.IsNotInFuture(target, "target");

            StringBuilder result = new StringBuilder();
            TimeSpan diff = (SystemTime.Now() - target);

            Action<int, string> format = (v, u) =>
                                                    {
                                                        if (v > 0)
                                                        {
                                                            if (result.Length > 0)
                                                            {
                                                                result.Append(", ");
                                                            }

                                                            string plural = (v > 1) ? "s" : string.Empty;

                                                            result.Append("{0} {1}{2}".FormatWith(v, u, plural));
                                                        }
                                                    };

            format(diff.Days, "day");
            format(diff.Hours, "hour");
            format(diff.Minutes, "minute");

            return (result.Length == 0) ? "few moments" : result.ToString();
        }
    }
}