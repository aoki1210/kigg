namespace Kigg.Web
{
    using Domain.Entities;

    public class StoryListUserViewData : StoryListViewData
    {
        public UserDetailTab SelectedTab
        {
            get;
            set;
        }

        public User TheUser
        {
            get;
            set;
        }
    }
}