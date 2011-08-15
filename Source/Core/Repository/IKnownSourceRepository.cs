namespace Kigg.Repository
{
    using DomainObjects;

    public interface IKnownSourceRepository : IRepository<KnownSource>
    {
        KnownSource FindMatching(string url);
    }
}