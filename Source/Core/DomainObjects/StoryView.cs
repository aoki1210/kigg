namespace Kigg.DomainObjects
{
    using System;

    public class StoryView
    {
        public virtual long Id { get; set; }

        public virtual Story ForStory { get; set; }

        public virtual string FromIPAddress { get; set; }

        public DateTime ViewedAt { get; set; }
    }
}