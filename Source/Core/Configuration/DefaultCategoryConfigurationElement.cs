namespace Kigg.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    public class DefaultCategoryConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["name"];
            }

            [DebuggerStepThrough]
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("uniqueName", IsRequired = true)]
        public string UniqueName
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["uniqueName"];
            }

            [DebuggerStepThrough]
            set
            {
                this["uniqueName"] = value;
            }
        }
    }
}