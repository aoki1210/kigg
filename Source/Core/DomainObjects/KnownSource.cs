namespace Kigg.DomainObjects
{
    using System;

    public class KnownSource : IEntity
    {
        public virtual long Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string Url { get; set; }
        protected virtual int GradeInternal { get; set; }
        public virtual KnownSourceGrade Grade
        {
            get { return (KnownSourceGrade)GradeInternal; }
            set { GradeInternal = (int)value; }
        }
    }
}