namespace Kigg.Domain.Entities
{
    public interface IUniqueNameEntity : IEntity
    {
        string UniqueName
        {
            get;
        }
    }
}