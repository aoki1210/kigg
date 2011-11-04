namespace Kigg.Repository
{
    using Domain.Entities;

    public interface IKnownSourceRepository : IEntityRepository<KnownSource>
    {
        KnownSource FindMatching(string url);
    }
}