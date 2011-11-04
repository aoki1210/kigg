namespace Kigg.Web
{
    using System.Collections.Generic;

    using Domain.Entities;

    public class StoryItemViewData
    {
        public Story Story
        {
            get;
            set;
        }

        public User User
        {
            get;
            set;
        }

        public string PromoteText
        {
            get;
            set;
        }

        public string DemoteText
        {
            get;
            set;
        }

        public string CountText
        {
            get;
            set;
        }

        public ICollection<string> SocialServices
        {
            get;
            set;
        }

        public bool DetailMode
        {
            get;
            set;
        }
    }
}