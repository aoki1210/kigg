namespace Kigg.Service
{
    using DomainObjects;

    public class UserScoreService : IUserScoreService
    {
        private readonly IUserScoreTable _userScoreTable;

        public UserScoreService(IUserScoreTable userScoreTable)
        {
            Check.Argument.IsNotNull(userScoreTable, "userScoreTable");

            _userScoreTable = userScoreTable;
        }

        public virtual void AccountActivated(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.IncreaseScoreBy(_userScoreTable.AccountActivated, UserAction.AccountActivated);
            }
        }

        public virtual void StorySubmitted(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            if (byUser.IsPublicUser())
            {
                byUser.IncreaseScoreBy(_userScoreTable.StorySubmitted, UserAction.StorySubmitted);
            }
        }

        public virtual void StoryViewed(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.IncreaseScoreBy(_userScoreTable.StoryViewed, UserAction.StoryViewed);
            }
        }

        public virtual void StoryPromoted(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                UserAction reason;
                decimal score;

                if (theStory.IsPublished())
                {
                    score = _userScoreTable.PublishedStoryPromoted;
                    reason = UserAction.PublishedStoryPromoted;
                }
                else
                {
                    score = _userScoreTable.UpcomingStoryPromoted;
                    reason = UserAction.UpcomingStoryPromoted;
                }

                byUser.IncreaseScoreBy(score, reason);
            }
        }

        public virtual void StoryDemoted(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                // It might not decrease the same value which was increased when promoting the story
                // depending upon the story status(e.g. published/upcoming), but who cares!!!
                UserAction reason;
                decimal score;

                if (theStory.IsPublished())
                {
                    score = _userScoreTable.PublishedStoryPromoted;
                    reason = UserAction.PublishedStoryDemoted;
                }
                else
                {
                    score = _userScoreTable.UpcomingStoryPromoted;
                    reason = UserAction.UpcomingStoryDemoted;
                }

                byUser.DecreaseScoreBy(score, reason);
            }
        }

        public virtual void StoryMarkedAsSpam(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.IncreaseScoreBy(_userScoreTable.StoryMarkedAsSpam, UserAction.StoryMarkedAsSpam);
            }
        }

        public virtual void StoryUnmarkedAsSpam(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.DecreaseScoreBy(_userScoreTable.StoryMarkedAsSpam, UserAction.StoryUnmarkedAsSpam);
            }
        }

        public virtual void StoryCommented(IStory theStory, IUser byUser)
        {
            Check.Argument.IsNotNull(theStory, "theStory");
            Check.Argument.IsNotNull(byUser, "byUser");

            if (CanChangeScoreForStory(theStory, byUser))
            {
                byUser.IncreaseScoreBy(_userScoreTable.StoryCommented, UserAction.StoryCommented);
            }
        }

        public virtual void StoryPublished(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.IncreaseScoreBy(_userScoreTable.StoryPublished, UserAction.StoryPublished);
            }
        }

        public virtual void StorySpammed(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.DecreaseScoreBy(_userScoreTable.StorySubmitted + _userScoreTable.SpamStorySubmitted, UserAction.SpamStorySubmitted);
            }
        }

        public virtual void StoryIncorrectlyMarkedAsSpam(IUser byUser)
        {
            Check.Argument.IsNotNull(byUser, "ofUser");

            if (byUser.IsPublicUser())
            {
                byUser.DecreaseScoreBy((_userScoreTable.StoryIncorrectlyMarkedAsSpam + _userScoreTable.StoryMarkedAsSpam), UserAction.StoryIncorrectlyMarkedAsSpam);
            }
        }

        public virtual void CommentSpammed(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.DecreaseScoreBy(_userScoreTable.StoryCommented + _userScoreTable.SpamCommentSubmitted, UserAction.SpamCommentSubmitted);
            }
        }

        public virtual void CommentMarkedAsOffended(IUser ofUser)
        {
            Check.Argument.IsNotNull(ofUser, "ofUser");

            if (ofUser.IsPublicUser())
            {
                ofUser.DecreaseScoreBy(_userScoreTable.StoryCommented, UserAction.CommentMarkedAsOffended);
            }
        }

        private static bool CanChangeScoreForStory(IStory theStory, IUser theUser)
        {
            return !theStory.HasExpired() && theUser.IsPublicUser();
        }
    }
}