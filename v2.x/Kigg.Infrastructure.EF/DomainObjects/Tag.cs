namespace Kigg.EF.DomainObjects
{
    using Infrastructure.DomainRepositoryExtensions;
    using Kigg.DomainObjects;

    public partial class Tag : ITag
    {
        private int _storyCount = -1;

        public int StoryCount
        {
            get
            {
                if (_storyCount == -1)
                {
                    _storyCount = this.GetStoryCount();
                }

                return _storyCount;
            }
        }
    }
}
