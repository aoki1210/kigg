namespace Kigg
{
    using System;
    using System.Globalization;

    public static class GlobalConstants
    {
        public static readonly DateTime ProductionDate = new DateTime(2008, 1, 11);

        [Obsolete("Obsolete use Kigg.Culture.Current instead")]
        public static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }
    }
}