using System;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    public abstract class BaseIntegrationFixture
    {
        protected const string _assemblyInfo = "Kigg.Infrastructure.EF, Version=2.2.0.0, Culture=neutral, PublicKeyToken=88117f6fba1a09d8";
        protected const string _edmFilesPrefix = "Kigg.Infrastructure.EF.EDM.DomainObjects";
        protected const string _edmConnStringFormat = "metadata=res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl;provider=System.Data.SqlClient;provider connection string=\"{2}\"";
        protected const string _providerConnString = "Data Source=.\\sqlexpress;Initial Catalog=KiGG;Integrated Security=True;MultipleActiveResultSets=True";

        protected string ConnectionString
        {
            get
            {
                return String.Format(_edmConnStringFormat, _assemblyInfo, _edmFilesPrefix, _providerConnString);
            }
        }
    }
}
