namespace Kigg.Web
{
    using Domain.Entities;

    public class StoryDetailViewData : BaseStoryViewData
    {
        public string Title
        {
            get;
            set;
        }

        public Story Story
        {
            get;
            set;
        }

        public bool CaptchaEnabled
        {
            get;
            set;
        }

        public DefaultColors CounterColors
        {
            get;
            set;
        }
    }
}