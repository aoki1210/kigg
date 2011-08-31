namespace Kigg.DomainObjects
{
    using System;

    public class MarkAsSpam
    {
        public virtual Story ForStory { get; set; }

        public virtual User ByUser { get; set; }

        public virtual string FromIPAddress { get; set; }

        public virtual DateTime MarkedAt { get; set; }
    }
}