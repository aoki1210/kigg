namespace Kigg.Repository
{
    using System;

    using DomainObjects;
    using Infrastructure;

    public class LoggingStoryRepository : DecoratedStoryRepository
    {
        public LoggingStoryRepository(IStoryRepository innerRepository) : base(innerRepository)
        {
        }

        public override void Add(Story entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Info("Adding story: {0}, {1}", entity.Id, entity.Title);
            base.Add(entity);
            Log.Info("Story added: {0}, {1}", entity.Id, entity.Title);
        }

        public override void Remove(Story entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Log.Warning("Removing story: {0}, {1}", entity.Id, entity.Title);
            base.Remove(entity);
            Log.Warning("Story removed: {0}, {1}", entity.Id, entity.Title);
        }

        public override Story FindById(long id)
        {
            //Check.Argument.IsNotEmpty(id, "id");

            Log.Info("Retrieving story with id: {0}", id);

            var result = base.FindById(id);

            if (result == null)
            {
                Log.Warning("Did not find any story with id: {0}", id);
            }
            else
            {
                Log.Info("Story retrieved with id: {0}", id);
            }

            return result;
        }

        public override Story FindByUniqueName(string uniqueName)
        {
            Check.Argument.IsNotNullOrEmpty(uniqueName, "uniqueName");

            Log.Info("Retrieving story with unique name: {0}", uniqueName);

            var result = base.FindByUniqueName(uniqueName);

            if (result == null)
            {
                Log.Warning("Did not find any story with unique name: {0}", uniqueName);
            }
            else
            {
                Log.Info("Story retrieved with unique name: {0}", uniqueName);
            }

            return result;
        }

        public override Story FindByUrl(string url)
        {
            Check.Argument.IsNotInvalidWebUrl(url, "url");

            Log.Info("Retrieving story with url: {0}", url);

            var result = base.FindByUrl(url);

            if (result == null)
            {
                Log.Warning("Did not find any story with url: {0}", url);
            }
            else
            {
                Log.Info("Story retrieved with url: {0}", url);
            }

            return result;
        }

        public override PagedResult<Story> FindPublished(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving published stories : {0}, {1}", start, max);

            var pagedResult = base.FindPublished(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any published story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("Published stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindPublishedByCategory(long categoryId, int start, int max)
        {
            //Check.Argument.IsNotEmpty(categoryId, "categoryId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving published stories for category : {0}, {1}, {2}", categoryId, start, max);

            var pagedResult = base.FindPublishedByCategory(categoryId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any published story for category : {0}, {1}, {2}", categoryId, start, max);
            }
            else
            {
                Log.Info("Published stories retrieved for category : {0}, {1}, {2}", categoryId, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindUpcoming(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving upcoming stories : {0}, {1}", start, max);

            var pagedResult = base.FindUpcoming(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any upcoming story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("Upcoming stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindNew(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving new stories : {0}, {1}", start, max);

            var pagedResult = base.FindNew(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any new story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("New stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindUnapproved(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving upapproved stories : {0}, {1}", start, max);

            var pagedResult = base.FindUnapproved(start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any upapproved story : {0}, {1}", start, max);
            }
            else
            {
                Log.Info("Upapproved stories retrieved : {0}, {1}", start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindPublishable(DateTime minimumDate, DateTime maximumDate, int start, int max)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving publishable stories : {0}, {1}", start, max);

            var pagedResult = base.FindPublishable(minimumDate, maximumDate, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any publishable story : {0}, {1}, {2}, {3}", minimumDate, maximumDate, start, max);
            }
            else
            {
                Log.Info("Publishable stories retrieved : {0}, {1}, {2}, {3}", minimumDate, maximumDate, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindByTag(long tagId, int start, int max)
        {
            //Check.Argument.IsNotEmpty(tagId, "tagId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories for tag : {0}, {1}, {2}", tagId, start, max);

            var pagedResult = base.FindByTag(tagId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story for tag : {0}, {1}, {2}", tagId, start, max);
            }
            else
            {
                Log.Info("Stories retrieved for tag : {0}, {1}, {2}", tagId, start, max);
            }

            return pagedResult;
        }
        public override PagedResult<Story> FindPostedByUser(long userId, int start, int max)
        {
            //Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories posted by user : {0}, {1}, {2}", userId, start, max);

            var pagedResult = base.FindPostedByUser(userId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story posted by user : {0}, {1}, {2}", userId, start, max);
            }
            else
            {
                Log.Info("Posted Stories retrieved for user : {0}, {1}, {2}", userId, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindPromotedByUser(long userId, int start, int max)
        {
            //Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories promoted by user : {0}, {1}, {2}", userId, start, max);

            var pagedResult = base.FindPromotedByUser(userId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story promoted by user : {0}, {1}, {2}", userId, start, max);
            }
            else
            {
                Log.Info("Promoted Stories retrieved for user : {0}, {1}, {2}", userId, start, max);
            }

            return pagedResult;
        }

        public override PagedResult<Story> FindCommentedByUser(long userId, int start, int max)
        {
            //Check.Argument.IsNotEmpty(userId, "userId");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            Log.Info("Retrieving stories commented by user : {0}, {1}, {2}", userId, start, max);

            var pagedResult = base.FindCommentedByUser(userId, start, max);

            if (pagedResult.IsEmpty)
            {
                Log.Warning("Did not find any story commented by user : {0}, {1}, {2}", userId, start, max);
            }
            else
            {
                Log.Info("Commented Stories retrieved for user : {0}, {1}, {2}", userId, start, max);
            }

            return pagedResult;
        }

        public override int CountPublished()
        {
            Log.Info("Retrieving published count");

            var result = base.CountPublished();

            Log.Info("Published count retrieved");

            return result;
        }

        public override int CountUpcoming()
        {
            Log.Info("Retrieving upcoming count");

            var result = base.CountUpcoming();

            Log.Info("Upcoming count retrieved");

            return result;
        }

        public override int CountByCategory(long categoryId)
        {
            //Check.Argument.IsNotEmpty(categoryId, "categoryId");

            Log.Info("Retrieving count for category : {0}", categoryId);

            var result = base.CountByCategory(categoryId);

            Log.Info("Count retrieved for category : {0}", categoryId);

            return result;
        }

        public override int CountByTag(long tagId)
        {
            //Check.Argument.IsNotEmpty(tagId, "tagId");

            Log.Info("Retrieving count for tag : {0}", tagId);

            var result = base.CountByTag(tagId);

            Log.Info("Count retrieved for tag : {0}", tagId);

            return result;
        }

        public override int CountNew()
        {
            Log.Info("Retrieving new count");

            var result = base.CountNew();

            Log.Info("New count retrieved");

            return result;
        }

        public override int CountUnapproved()
        {
            Log.Info("Retrieving unapproved count");

            var result = base.CountUnapproved();

            Log.Info("Unapproved count retrieved");

            return result;
        }

        public override int CountPublishable(DateTime minimumDate, DateTime maximumDate)
        {
            Check.Argument.IsNotInFuture(minimumDate, "minimumDate");
            Check.Argument.IsNotInFuture(maximumDate, "maximumDate");

            Log.Info("Retrieving publishable count: {0}-{1}", minimumDate, maximumDate);

            var result = base.CountPublishable(minimumDate, maximumDate);

            Log.Info("Publishable count retrieved: {0}-{1}", minimumDate, maximumDate);

            return result;
        }

        public override int CountPostedByUser(long userId)
        {
            Log.Info("Retrieving posted by user count: {0}".FormatWith(userId));

            var result = base.CountPostedByUser(userId);

            Log.Info("posted by user count: {0}".FormatWith(userId));

            return result;
        }
    }
}