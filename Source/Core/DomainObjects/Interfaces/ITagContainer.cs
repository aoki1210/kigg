namespace Kigg.DomainObjects
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

        void RemoveAllTags();

        bool ContainsTag(Tag tag);
    }
}