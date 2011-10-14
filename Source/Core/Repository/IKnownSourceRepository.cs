namespace Kigg.Repository
{
    using DomainObjects;

    public interface IKnownSourceRepository : IEntityRepository<KnownSource>
    {
        KnownSource FindMatching(string url);
    }
}