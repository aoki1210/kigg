namespace Kigg.Repository
{
    using System.Collections.Generic;

    using DomainObjects;

    public interface ICategoryRepository : IUniqueNameEntityRepository<Category>
    {
        ICollection<Category> FindAll();
    }
}