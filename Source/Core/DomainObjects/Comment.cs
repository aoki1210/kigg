namespace Kigg.DomainObjects
{
    using System;

    public class Comment : IEntity
    {
        public virtual long Id { get; set; }

        public virtual Story ForStory { get; set; }

        public virtual string HtmlBody { get; set; }

        public virtual string TextBody { get; set; }

        public virtual User ByUser { get; set; }

        public virtual string FromIPAddress { get; set; }

        public virtual bool IsOffended { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public void MarkAsOffended()
        {
            throw new NotImplementedException();
        }
    }
}