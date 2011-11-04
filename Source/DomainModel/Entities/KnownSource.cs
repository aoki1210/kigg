namespace Kigg.Domain.Entities
{
    using System;

    public class KnownSource : IEntity
    {
        public virtual long Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string Url { get; set; }
        public virtual KnownSourceGrade Grade { get; set; }
    }
}