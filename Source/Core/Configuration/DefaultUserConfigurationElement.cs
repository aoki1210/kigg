namespace Kigg.Configuration
{
    using System.Configuration;
    using System.Diagnostics;

    using Domain.Entities;

    public class DefaultUserConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("userName", IsRequired = true, IsKey = true)]
        public string UserName
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["userName"];
            }

            [DebuggerStepThrough]
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["password"];
            }

            [DebuggerStepThrough]
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("displayName")]
        public string DisplayName
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["displayName"];
            }

            [DebuggerStepThrough]
            set
            {
                this["displayName"] = value;
            }
        }

        [ConfigurationProperty("description")]
        public string Description
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["description"];
            }

            [DebuggerStepThrough]
            set
            {
                this["description"] = value;
            }
        }

        [ConfigurationProperty("email")]
        public string Email
        {
            [DebuggerStepThrough]
            get
            {
                return (string)this["email"];
            }

            [DebuggerStepThrough]
            set
            {
                this["email"] = value;
            }
        }

        [ConfigurationProperty("role", DefaultValue = Role.Administrator)]
        public Role Role
        {
            [DebuggerStepThrough]
            get
            {
                return (Role)this["role"];
            }

            [DebuggerStepThrough]
            set
            {
                this["role"] = value;
            }
        }
    }
}