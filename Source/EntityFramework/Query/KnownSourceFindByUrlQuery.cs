using System.Linq;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EntityFramework.Query
{
    public class KnownSourceFindByUrlQuery : QueryBase<KnownSource>
    {
        private readonly string url;
        public KnownSourceFindByUrlQuery(KiggDbContext context, string url)
            : base(context)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");

            this.url = url;
        }

        public override KnownSource Execute()
        {
            return context.KnownSources.Single(s => s.Url == url);
        }
    }
}
