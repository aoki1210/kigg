namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;

    using Query;    
    using Repository;
    using Domain.Entities;

    public class UserRepository : EntityRepositoryBase<User>, IUserRepository
    {
        public UserRepository(IKiggDbFactory dbContextFactory, IQueryFactory queryFactory)
            : base(dbContextFactory, queryFactory)
        {
        }
        
        public override void Add(User entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            // Can't allow duplicate user name
            if (FindByUserName(entity.UserName) != null)
            {
                throw new ArgumentException("\"{0}\" already exits. Specifiy a diffrent user name.".FormatWith(entity.UserName));
            }

            // Ensure that same email doesn't exist for non openid user
            if (!entity.IsOpenIDAccount())
            {
                if (FindByEmail(entity.Email) != null)
                {
                    throw new ArgumentException("\"{0}\" already exits. Specifiy a diffrent email address.".FormatWith(entity.Email));
                }
            }

            base.Add(entity);
        }

        public override void Remove(User entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            //user.RemoveAllTags();
            //user.RemoveAllCommentSubscriptions();

            //Database.DeleteAllOnSubmit(Database.StoryViewDataSource.Where(v => v.Story.User.Id == user.Id));
            //Database.DeleteAllOnSubmit(Database.CommentDataSource.Where(c => c.User.Id == user.Id || c.Story.User.Id == user.Id));
            //Database.DeleteAllOnSubmit(Database.VoteDataSource.Where(v => v.User.Id == user.Id || v.Story.User.Id == user.Id));
            //Database.DeleteAllOnSubmit(Database.MarkAsSpamDataSource.Where(s => s.User.Id == user.Id || s.Story.User.Id == user.Id));

            ////Convert to List immediatly to avoid issues with databases
            ////that do not support MARS 
            //var submittedStories = Database.StoryDataSource
            //                               .Where(s => s.User.Id == user.Id)
            //                               .ToList()
            //                               .AsReadOnly();

            //submittedStories.ForEach(s => s.RemoveAllTags());
            //submittedStories.ForEach(s => s.RemoveAllCommentSubscribers());
            //submittedStories.ForEach(s => Database.DeleteOnSubmit(s));

            base.Remove(entity);
        }

        public User FindByUserName(string userName)
        {
            Check.Argument.IsNotNullOrEmpty(userName, "userName");

            var query = QueryFactory.CreateFindUserByUserName(userName);

            return query.Execute();
        }

        public User FindByEmail(string email)
        {
            Check.Argument.IsNotInvalidEmail(email, "email");
            
            var emailLowerInvariant = email.ToLowerInvariant();

            var query = QueryFactory.CreateFindUserByEmail(emailLowerInvariant);

            return query.Execute();
        }

        public decimal FindScoreById(long id, DateTime startTimestamp, DateTime endTimestamp)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");

            var query = QueryFactory.CreateCalculateUserScoreById(id, startTimestamp, endTimestamp);

            return query.Execute();
        }

        public PagedResult<User> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max)
        {
            Check.Argument.IsNotInFuture(startTimestamp, "startTimestamp");
            Check.Argument.IsNotInFuture(endTimestamp, "endTimestamp");
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var query = QueryFactory.CreateFindTopScoredUsers(startTimestamp, endTimestamp, start, max);

            return CreatePagedResult(query);
        }
     
        public PagedResult<User> FindAll(int start, int max)
        {
            Check.Argument.IsNotNegative(start, "start");
            Check.Argument.IsNotNegative(max, "max");

            var query = QueryFactory.CreateFindAllUsers(start, max, u => u.UserName);

            return CreatePagedResult(query);
        }

        public IEnumerable<string> FindIPAddresses(long id)
        {
            throw new NotImplementedException();
            //Check.Argument.IsNotEmpty(id, "id");
            //ICollection<string> all;
            //if(DataContext!=null)
            //{
            //    all = FindUserIpAddressesById(DataContext, id).ToList().AsReadOnly();
            //}
            //else
            //{
            //    IQueryable<string> storyIps = Database.StoryDataSource
            //                                      .Where(s => s.User.Id == id)
            //                                      .Select(s => s.IpAddress);

            //    IQueryable<string> voteIps = Database.VoteDataSource
            //                                         .Where(v => v.UserId == id)
            //                                         .Select(v => v.IpAddress);

            //    IQueryable<string> commentIps = Database.CommentDataSource
            //                                            .Where(c => c.User.Id == id)
            //                                            .Select(c => c.IpAddress);

            //    IQueryable<string> markAsSpamsIps = Database.MarkAsSpamDataSource
            //                                                .Where(s => s.UserId == id)
            //                                                .Select(s => s.IpAddress);

            //    all = storyIps.Union(voteIps)
            //                  .Union(commentIps)
            //                  .Union(markAsSpamsIps)
            //                  .Distinct()
            //                  .ToList()
            //                  .AsReadOnly();
            //}


            //return all;
        }
    }
}