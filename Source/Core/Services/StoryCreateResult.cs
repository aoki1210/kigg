namespace Kigg.Service
{
    using Domain.Entities;

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