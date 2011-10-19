namespace Kigg.Infrastructure.EntityFramework
{
    using System;

    using Query;
    using Repository;
    using DomainObjects;

    public class KnownSourceRepository : EntityRepositoryBase<KnownSource>, IKnownSourceRepository
    {
        public KnownSourceRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }

        public override void Add(KnownSource entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            if (Exists(ks => ks.Url == entity.Url))
            {
                throw new ArgumentException("\"{0}\" source already exits. Specifiy a diffrent url.".FormatWith(entity.Url), "entity");
            }

            base.Add(entity);
        }

        public KnownSource FindMatching(string url)
        {
            Check.Argument.IsNotNullOrEmpty(url, "url");
            Check.Argument.IsNotInvalidWebUrl(url, "url");
            
            var query = QueryFactory.CreateFindKnownSourceByUrl(url);
            
            return query.Execute();
        }
    }
}