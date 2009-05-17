﻿using System;
using System.Linq;
using System.Data;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class StoryRepositoryFixture : BaseIntegrationFixture
    {
        private readonly StoryRepository _storyRepository;

        public StoryRepositoryFixture()
        {
            _storyRepository = new StoryRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new StoryRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var user = GetAnyExistingUser();
                var category = _database.CategoryDataSource.First();
                var story = CreateNewStory(category, user, "192.168.0.1", "dummy title", "dummy desc", "http://somesite.net/story.aspx");
                _storyRepository.Add(story);
                Assert.Equal(EntityState.Added, story.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, story.EntityState);
            }
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            using (new TransactionScope())
            {
                var story = CreateNewStory();
                _storyRepository.Add(story);
                _database.SubmitChanges();

                Assert.Throws<ArgumentException>(() => _storyRepository.Add(story));    
            }
            
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var user = (User) _domainFactory.CreateUser("dummyuser", "dummyuser@mail.com", String.Empty);
                var category = _domainFactory.CreateCategory("dummycategory");
                var story = _domainFactory.CreateStory(category, user, "192.168.0.1", "dummy title", "dummy Desc", "http://kiGG.net/story.aspx");
                var tag = _domainFactory.CreateTag("DummyTag");
                story.AddTag(tag);
                user.AddTag(tag);

                #pragma warning disable 168
                var comment = _domainFactory.CreateComment(story, "comment", SystemTime.Now(), user, "192.168.0.2");
                var vote = _domainFactory.CreateStoryVote(story, SystemTime.Now(), user, "192.168.0.1");
                var spamMark = _domainFactory.CreateMarkAsSpam(story, SystemTime.Now(), user, "192.168.0.3");
                #pragma warning restore 168

                _storyRepository.Add(story);
                _database.SubmitChanges();

                _storyRepository.Remove(story);
                _database.SubmitChanges();
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_Story()
        {
            var id = GetAnyExistingStory().Id;
            var story = _storyRepository.FindById(id);
            Assert.Equal(id, story.Id);
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Story()
        {
            var uniqueName = GetAnyExistingStory().UniqueName;
            var story = _storyRepository.FindByUniqueName(uniqueName);
            Assert.Equal(uniqueName, story.UniqueName);
        }

        [Fact]
        public void FindByUrl_Should_Return_Correct_Story()
        {
            var url = GetAnyExistingStory().Url;
            var story = _storyRepository.FindByUrl(url);
            Assert.Equal(url, story.Url);
        }

        [Fact]
        public void FindByPublished_Should_Return_Paged_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true,false, true);
                
                _database.SubmitChanges();

                var pagedResult = _storyRepository.FindPublished(0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(10, pagedResult.Total);
            }
        }

        [Fact]
        public void FindPublishedByCategory_Should_Return_Paged_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();

                var categoryName = CreateNewCategory().Name;
                var category = _database.CategoryDataSource.First(c => c.Name == categoryName);
                var pagedResult = _storyRepository.FindPublishedByCategory(category.Id, 0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(10, pagedResult.Total);
            }
        }

        [Fact]
        public void FindUpcoming_Should_Return_Paged_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false,true, true);
                _database.SubmitChanges();
                var pagedResult = _storyRepository.FindUpcoming(0, 5);
                Assert.Equal(5, pagedResult.Result.Count);
                Assert.True(pagedResult.Total >= 10);
            }
        }
        [Fact]
        public void FindNew_Should_Return_Paged_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false,true, true);
                _database.SubmitChanges();
                var pagedResult = _storyRepository.FindNew(0, 5);
                Assert.Equal(5, pagedResult.Result.Count);
                Assert.True(pagedResult.Total >= 10);
            }
        }
        [Fact]
        public void FindUnapproved_Should_Return_Paged_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false, true, false);
                _database.SubmitChanges();
                var pagedResult = _storyRepository.FindUnapproved(0, 5);
                Assert.Equal(5, pagedResult.Result.Count);
                Assert.True(pagedResult.Total >= 10);
            }
        }
        
        [Fact]
        public void FindPublishable_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false, true, true);
                _database.SubmitChanges();
                var pagedResult = _storyRepository.FindPublishable(SystemTime.Now().AddDays(-7), SystemTime.Now().AddHours(-4), 0, 5);
                Assert.Equal(5, pagedResult.Result.Count);
                Assert.True(pagedResult.Total >= 10);
            }
        }
        
        [Fact]
        public void FindByTag_Should_Return_Paged_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();

                var tagName = CreateNewTag().Name;
                var tag = _database.TagDataSource.First(c => c.Name == tagName);
                var pagedResult = _storyRepository.FindByTag(tag.Id, 0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(10, pagedResult.Total);
            }
        }

        [Fact]
        public void Search_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();

                var pagedResult = _storyRepository.Search("dummy", 0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.True(pagedResult.Total >= 10);
            }
        }

        [Fact]
        public void FindPostedByUser_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();
                var userName = CreateNewUser().UserName;
                var id = _database.UserDataSource.First(u => u.UserName == userName).Id;
                var pagedResult = _storyRepository.FindPostedByUser(id, 0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(10, pagedResult.Total);
            }
        }
        
        [Fact]
        public void FindPromotedByUser_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();
                var userName = CreateNewUser().UserName;
                var stories = _database.StoryDataSource.Where(s => s.User.UserName == userName);

                var newUser = (User)_domainFactory.CreateUser("promoterUser", "promoterUser@mail.com", "Pa$$w0rd");
                
                stories.ForEach(s=>s.StoryVotesInternal.Add(new StoryVote{User = newUser, Story = s, IpAddress = "192.168.0.5", Timestamp = SystemTime.Now()}));
                _database.SubmitChanges();

                var pagedResult = _storyRepository.FindPromotedByUser(newUser.Id, 0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(10, pagedResult.Total);
            }
        }

        [Fact]
        public void FindCommentedByUser_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();
                var userName = CreateNewUser().UserName;
                var stories = _database.StoryDataSource.Where(s => s.User.UserName == userName);

                var newUser = (User)_domainFactory.CreateUser("commentUser", "commentUser@mail.com", "Pa$$w0rd");
                var anotherUser = (User)_domainFactory.CreateUser("anothercommentUser", "anothercommentUser@mail.com", "Pa$$w0rd");
                var rnd = new Random();
                stories.ForEach(s => s.StoryCommentsInternal.Add((StoryComment)_domainFactory.CreateComment(s, "content", SystemTime.Now().AddDays(rnd.Next(-10, -1)), newUser, "192.168.0.1")));
                stories.ForEach(s => s.StoryCommentsInternal.Add((StoryComment)_domainFactory.CreateComment(s, "content2", SystemTime.Now().AddDays(rnd.Next(-10, -1)), newUser, "192.168.0.4")));
                stories.ForEach(s => s.StoryCommentsInternal.Add((StoryComment)_domainFactory.CreateComment(s, "content2", SystemTime.Now().AddDays(rnd.Next(-10, -1)), anotherUser, "192.168.0.4")));
                _database.SubmitChanges();

                var pagedResult = _storyRepository.FindCommentedByUser(newUser.Id, 0, 5);

                Assert.Equal(5, pagedResult.Result.Count);
                Assert.Equal(10, pagedResult.Total);
            }
        }

        [Fact]
        public void CountByPublished_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();


                var count = _storyRepository.CountByPublished();

                Assert.Equal(10, count);
            }
        }

        [Fact]
        public void CountByUpcoming_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false, false, true);

                _database.SubmitChanges();

                var count = _storyRepository.CountByUpcoming();

                Assert.True(count >= 10);
            }
        }

        [Fact]
        public void CountByCategory_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();
                var categoryName = CreateNewCategory().Name;
                var id = _database.CategoryDataSource.First(c => c.Name == categoryName).Id;
                var count = _storyRepository.CountByCategory(id);

                Assert.Equal(10, count);
            }
        }

        [Fact]
        public void CountByTag_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(true, false, true);

                _database.SubmitChanges();
                var tagName = CreateNewTag().Name;
                var id = _database.TagDataSource.First(t => t.Name == tagName).Id;
                var count = _storyRepository.CountByTag(id);

                Assert.Equal(10, count);
            }
        }

        [Fact]
        public void CountByNew_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false, true, true);

                _database.SubmitChanges();
                var count = _storyRepository.CountByNew();

                Assert.True(count >= 10);
            }    
        }

        [Fact]
        public void CountByUnapproved_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false, true, false);

                _database.SubmitChanges();
                var count = _storyRepository.CountByUnapproved();

                Assert.True(count >= 10);
            }   
        }

        [Fact]
        public void CountByPublishable_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false, true, true);

                _database.SubmitChanges();
                var count = _storyRepository.CountByPublishable(SystemTime.Now().AddDays(-10),SystemTime.Now());

                Assert.True(count >= 10);
            }  
        }
        [Fact]
        public void CountPostedByUser_Should_Return_Correct_Result()
        {
            using (new TransactionScope())
            {
                GenerateStories(false, true, true);

                _database.SubmitChanges();

                var userName = CreateNewUser().UserName;
                var id = _database.UserDataSource.First(u => u.UserName == userName).Id;

                var count = _storyRepository.CountPostedByUser(id);

                Assert.Equal(10, count);
            }  
        }

        private void GenerateStories(bool publish, bool markNew, bool approve)
        {
            var rnd = new Random();
            var category = CreateNewCategory();
            var user = CreateNewUser();//GetAnyExistingUser();
            var tag = CreateNewTag();
            for (var i = 0; i < 10; i++)
            {
                var title = "dummy title " + i;
                var url = String.Format("http://blog.net/story{0}.html", i);
                var story = CreateNewStory(category, user, "192.168.0.1", title, "dummy desc", url);
                story.AddTag(tag);
                if (approve)
                {
                    story.Approve(SystemTime.Now().AddDays(-6));
                }
                if(publish)
                {
                    story.Publish(SystemTime.Now().AddDays(-rnd.Next(1, 5)), rnd.Next(1, 10));
                }
                if(markNew)
                {
                    story.LastProcessedAt = null;
                }
                _storyRepository.Add(story);
            }
        }
    }
}
