namespace Kigg.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public class Tag : IUniqueNameEntity
    {
        public virtual long Id { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual string UniqueName { get; set; }
        
        public virtual DateTime CreatedAt { get; set; }

        internal virtual ICollection<Story> Stories { get; set; }

        public int StoryCount
        {
            get { throw new NotImplementedException(); }
        }
    }
}