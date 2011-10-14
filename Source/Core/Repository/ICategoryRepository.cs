namespace Kigg.Repository
{
    using System.Collections.Generic;

    using DomainObjects;

    public interface ICategoryRepository : IUniqueNameEntityRepository<Category>
    {
        Category FindByName(string name);

        IEnumerable<Category> FindAll();
    }
}