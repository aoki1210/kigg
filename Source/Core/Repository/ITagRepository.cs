namespace Kigg.Repository
{
    using System.Collections.Generic;

    using DomainObjects;

    public interface ITagRepository : IUniqueNameEntityRepository<Tag>
    {
        Tag FindByName(string name);

        ICollection<Tag> FindMatching(string name, int max);

        ICollection<Tag> FindByUsage(int top);

        ICollection<Tag> FindAll();
    }
}