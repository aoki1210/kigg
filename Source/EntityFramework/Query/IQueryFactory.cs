namespace Kigg.Infrastructure.EntityFramework.Query
{
    using DomainObjects;

    public interface IQueryFactory
    {
        bool UseCompiled { get; }
        
        IQuery<Tag> CreateFindTagByUniqueName(string uniqueName);
        IQuery<Tag> CreateFindTagByName(string name);
        IOrderedQuery<Tag> CreateFindTagsByMatchingName(string name, int max);
        IOrderedQuery<Tag> CreateFindTagsByUsage(int max);
    }

}