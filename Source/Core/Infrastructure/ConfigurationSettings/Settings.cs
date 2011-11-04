namespace Kigg.Infrastructure
{
    using System.Collections.Generic;
    
    using Domain.Entities;

    public class Settings
    {
        public Settings(ThumbnailSettings thumbnail, /*AssetSettings asset,*/ TwitterSettings twitter = null, IEnumerable<User> defaultUsers = null, IEnumerable<Category> defaultCategories = null)
        {
            Check.Argument.IsNotNull(thumbnail, "thumbnail");

            Thumbnail = thumbnail;
            Twitter = twitter;
            //Asset = asset;
            DefaultUsers = defaultUsers ?? new User[0];
            DefaultCategories = defaultCategories ?? new Category[0];
        }

        internal Settings()
        {
        }

        public ThumbnailSettings Thumbnail
        {
            get;
            internal set;
        }

        public TwitterSettings Twitter
        {
            get;
            internal set;
        }
        /*
        public AssetSettings Asset
        {
            get;
            internal set;
        }
        */
        public IEnumerable<User> DefaultUsers
        {
            get;
            internal set;
        }

        public IEnumerable<Category> DefaultCategories
        {
            get;
            internal set;
        }

    }
}
