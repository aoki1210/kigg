namespace Kigg.EF.Repository
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    using Infrastructure;
    
    public class ConnectionString : IConnectionString
    {
        private readonly string _providerConnectionString;
        private readonly string _edmConnectionString;
        
        private const string _edmConnStringFormat = "metadata=res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl;provider=System.Data.SqlClient;provider connection string=\"{2}\"";
        private const string _edmFilesPrefix = "Kigg.Infrastructure.EF.EDM.DomainObjects";
        
        public ConnectionString(IConfigurationManager configuration, string name)
        {
            Check.Argument.IsNotNull(configuration, "configuration");
            Check.Argument.IsNotEmpty(name, "name");

            _providerConnectionString = configuration.ConnectionStrings(name);

            _edmConnectionString = String.Format(CultureInfo.InvariantCulture,
                                                 _edmConnStringFormat,
                                                 GetType().Assembly.FullName, 
                                                 _edmFilesPrefix, 
                                                 _providerConnectionString);

        }

        public string Value
        {
            [DebuggerStepThrough]
            get
            {
                return _edmConnectionString;
            }
        }
    }
}
