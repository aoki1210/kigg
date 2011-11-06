namespace Kigg.Domain.Entities
{
    using System;

    public class SpamVote : IDomainObject
    {
        public virtual long UserId { get; set; }

        public virtual long StoryId { get; set; }

        public virtual Story ForStory { get; set; }

        public virtual User ByUser { get; set; }

        public virtual string FromIPAddress { get; set; }

        public virtual DateTime MarkedAt { get; set; }
    }
}