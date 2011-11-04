namespace Kigg.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    public class ThumbnailConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("endpoint", IsRequired = true)]
        public string Endpoint
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["endpoint"];
            }

            [DebuggerStepThrough]
            set
            {
                base["endpoint"] = value;
            }
        }

        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string ApiKey
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["apiKey"];
            }

            [DebuggerStepThrough]
            set
            {
                base["apiKey"] = value;
            }
        }
    }
}