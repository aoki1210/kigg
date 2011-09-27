namespace Kigg.DomainObjects
{
    public class CommentSubscribtion : IDomainObject
    {
        public virtual Story ForStory { get; set; }

        public virtual User ByUser { get; set; }
    }
}