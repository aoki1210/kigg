namespace Kigg.Domain.Entities
{
    using System.Collections.Generic;

    public interface ITagContainer
    {
        ICollection<Tag> Tags
        {
            get;
        }
        
        void AddTag(Tag tag);

        void RemoveTag(Tag tag);

        bool ContainsTag(Tag tag);
    }
}