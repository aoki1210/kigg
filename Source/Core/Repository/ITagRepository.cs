namespace Kigg.Repository
{
    using System.Collections.Generic;

    using Domain.Entities;

    public interface ITagRepository : IUniqueNameEntityRepository<Tag>
    {
        Tag FindByName(string name);

        IEnumerable<Tag> FindMatching(string name, int max);

        IEnumerable<Tag> FindByUsage(int top);

        IEnumerable<Tag> FindAll();
    }
}