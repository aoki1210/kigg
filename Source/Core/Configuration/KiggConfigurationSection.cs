namespace Kigg.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    public class KiggConfigurationSection : ConfigurationSection
    {
        private static string sectionName = "kigg";

        public static string SectionName
        {
            [DebuggerStepThrough]
            get
            {
                return sectionName;
            }

            [DebuggerStepThrough]
            set
            {
                Check.Argument.IsNotNullOrEmpty(value, "value");

                sectionName = value;
            }
        }

        [ConfigurationProperty("thumbnail", IsRequired = true)]
        public ThumbnailConfigurationElement Thumbnail
        {
            [DebuggerStepThrough]
            get
            {
                return base["thumbnail"] as ThumbnailConfigurationElement;
            }
        }

        [ConfigurationProperty("twitter")]
        public TwitterConfigurationElement Twitter
        {
            [DebuggerStepThrough]
            get
            {
                return base["twitter"] as TwitterConfigurationElement;
            }
        }

        [ConfigurationProperty("users")]
        public DefaultUserConfigurationElementCollection DefaultUsers
        {
            [DebuggerStepThrough]
            get
            {
                return this["users"] as DefaultUserConfigurationElementCollection ?? new DefaultUserConfigurationElementCollection();
            }
        }

        [ConfigurationProperty("categories")]
        public DefaultCategoryConfigurationElementCollection DefaultCategories
        {
            [DebuggerStepThrough]
            get
            {
                return this["categories"] as DefaultCategoryConfigurationElementCollection ?? new DefaultCategoryConfigurationElementCollection();
            }
        }

        /*
        [ConfigurationProperty("assets")]
        public AssetConfigurationElementCollection Assets
        {
            [DebuggerStepThrough]
            get
            {
                return this["assets"] as AssetConfigurationElementCollection ?? new AssetConfigurationElementCollection();
            }
        }
       */
    }
}