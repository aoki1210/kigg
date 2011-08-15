namespace Kigg.Service
{
    using DomainObjects;

    public class StoryCreateResult : BaseServiceResult
    {
        public Story NewStory
        {
            get;
            set;
        }

        public string DetailUrl
        {
            get;
            set;
        }
    }
}