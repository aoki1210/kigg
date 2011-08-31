namespace Kigg.DomainObjects
{
    using System;
    public class Category : IUniqueNameEntity
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
        
        public virtual string UniqueName { get; set; }
        
        public virtual DateTime CreatedAt { get; set; }
    }
}