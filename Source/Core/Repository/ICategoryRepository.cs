namespace Kigg.Repository
{
    using System.Collections.Generic;

    using Domain.Entities;

    public interface ICategoryRepository : IUniqueNameEntityRepository<Category>
    {
        Category FindByName(string name);

        IEnumerable<Category> FindAll();
    }
}