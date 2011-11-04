namespace Kigg.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    public class TwitterConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["userName"];
            }

            [DebuggerStepThrough]
            set
            {
                base["userName"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["password"];
            }

            [DebuggerStepThrough]
            set
            {
                base["password"] = value;
            }
        }

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

        [ConfigurationProperty("maximumMessageLength", IsRequired = true)]
        public int MaximumMessageLength
        {
            [DebuggerStepThrough]
            get
            {
                return (int)base["maximumMessageLength"];
            }

            [DebuggerStepThrough]
            set
            {
                base["maximumMessageLength"] = value;
            }
        }

        [ConfigurationProperty("messageTemplate", IsRequired = true)]
        public string MessageTemplate
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["messageTemplate"];
            }

            [DebuggerStepThrough]
            set
            {
                base["messageTemplate"] = value;
            }
        }

        [ConfigurationProperty("recipients", IsRequired = true)]
        public string Recipients
        {
            [DebuggerStepThrough]
            get
            {
                return (string)base["recipients"];
            }

            [DebuggerStepThrough]
            set
            {
                base["recipients"] = value;
            }
        }
    }
}