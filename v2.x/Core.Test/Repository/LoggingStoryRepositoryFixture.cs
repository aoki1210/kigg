using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class LoggingStoryRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<IStoryRepository> _innerRepository;
        private readonly LoggingStoryRepository _loggingRepository;

        public LoggingStoryRepositoryFixture()
        {
            _innerRepository = new Mock<IStoryRepository>();
            _loggingRepository = new LoggingStoryRepository(_innerRepository.Object);
        }

        [Fact]
        public void Add_Should_Log_Info()
        {
            Add();

            log.Verify();
        }

        [Fact]
        public void Add_Should_Use_InnerRepository()
        {
            Add();

            _innerRepository.Verify();
        }

        [Fact]
        public void Remove_Should_Log_Warning()
        {
            Remove();

            log.Verify();
        }

        [Fact]
        public void Remove_Should_Use_InnerRepository()
        {
            Remove();

            _innerRepository.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Info_When_Story_Exists()
        {
            FindById(CreateStubStory());

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Log_Warning_When_Story_Does_Not_Exist()
        {
            FindById(null);

            log.Verify();
        }

        [Fact]
        public void FindById_Should_Use_InnerRepository()
        {
            FindById(CreateStubStory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Info_When_Story_Exists()
        {
            FindByUniqueName(CreateStubStory());

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Log_Warning_When_Story_Does_Not_Exist()
        {
            FindByUniqueName(null);

            log.Verify();
        }

        [Fact]
        public void FindByUniqueName_Should_Use_InnerRepository()
        {
            FindByUniqueName(CreateStubStory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByUrl_Should_Use_InnerRepository_And_Log_When_Story_Exists()
        {
        }

        [Fact]
        public void FindByUrl_Should_Use_InnerRepository_And_Log_When_Story_Does_Not_Exist()
        {
            _innerRepository.Expect(r => r.FindByUrl(It.IsAny<String>())).Returns((IStory) null).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            _loggingRepository.FindByUrl("http://story.com");
        }

        [Fact]
        public void FindByUrl_Should_Log_Info_When_Story_Exists()
        {
            FindByUrl(CreateStubStory());

            log.Verify();
        }

        [Fact]
        public void FindByUrl_Should_Log_Warning_When_Story_Does_Not_Exist()
        {
            FindByUrl(null);

            log.Verify();
        }

        [Fact]
        public void FindByUrl_Should_Use_InnerRepository()
        {
            FindByUrl(CreateStubStory());

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPublished_Should_Log_Info_When_Stories_Exist()
        {
            FindPublished(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPublished_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPublished(null);

            log.Verify();
        }

        [Fact]
        public void FindPublished_Should_Use_InnerRepository()
        {
            FindPublished(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPublishedByCategory_Should_Log_Info_When_Stories_Exist()
        {
            FindPublishedByCategory(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPublishedByCategory_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPublishedByCategory(null);

            log.Verify();
        }

        [Fact]
        public void FindPublishedByCategory_Should_Use_InnerRepository()
        {
            FindPublishedByCategory(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Log_Info_When_Stories_Exist()
        {
            FindUpcoming(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindUpcoming(null);

            log.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Use_InnerRepository()
        {
            FindUpcoming(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindByTag_Should_Log_Info_When_Stories_Exist()
        {
            FindByTag(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindByTag_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindByTag(null);

            log.Verify();
        }

        [Fact]
        public void FindByTag_Should_Use_InnerRepository()
        {
            FindByTag(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void Search_Should_Log_Info_When_Stories_Exist()
        {
            Search(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void Search_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            Search(null);

            log.Verify();
        }

        [Fact]
        public void Search_Should_Use_InnerRepository()
        {
            Search(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPostedByUser_Should_Log_Info_When_Stories_Exist()
        {
            FindPostedByUser(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPostedByUser_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPostedByUser(null);

            log.Verify();
        }

        [Fact]
        public void FindPostedByUser_Should_Use_InnerRepository()
        {
            FindPostedByUser(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPromotedByUser_Should_Log_Info_When_Stories_Exist()
        {
            FindPromotedByUser(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPromotedByUser_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPromotedByUser(null);

            log.Verify();
        }

        [Fact]
        public void FindPromotedByUser_Should_Use_InnerRepository()
        {
            FindPromotedByUser(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindCommentedByUser_Should_Log_Info_When_Stories_Exist()
        {
            FindCommentedByUser(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindCommentedByUser_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindCommentedByUser(null);

            log.Verify();
        }

        [Fact]
        public void FindCommentedByUser_Should_Use_InnerRepository()
        {
            FindCommentedByUser(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void FindPublishable_Should_Log_Info_When_Stories_Exist()
        {
            FindPublishable(new[] { CreateStubStory() });

            log.Verify();
        }

        [Fact]
        public void FindPublishable_Should_Log_Warning_When_Stories_Do_Not_Exist()
        {
            FindPublishable(null);

            log.Verify();
        }

        [Fact]
        public void FindPublishable_Should_Use_InnerRepository()
        {
            FindPublishable(new[] { CreateStubStory() });

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByPublished_Should_Log_Info()
        {
            CountByPublished();

            log.Verify();
        }

        [Fact]
        public void CountByPublished_Should_Use_InnerRepository()
        {
            CountByPublished();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByUpcoming_Should_Log_Info()
        {
            CountByUpcoming();

            log.Verify();
        }

        [Fact]
        public void CountByUpcoming_Should_Use_InnerRepository()
        {
            CountByUpcoming();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByCategory_Should_Log()
        {
            CountByCategory();

            log.Verify();
        }

        [Fact]
        public void CountByCategory_Should_Use_InnerRepository()
        {
            CountByCategory();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByTag_Should_Log()
        {
            CountByTag();

            log.Verify();
        }

        [Fact]
        public void CountByTag_Should_Use_InnerRepository()
        {
            CountByTag();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByNew_Should_Log()
        {
            CountByNew();

            log.Verify();
        }

        [Fact]
        public void CountByNew_Should_Use_InnerRepository()
        {
            CountByNew();

            _innerRepository.Verify();
        }

        [Fact]
        public void CountByNew_Should_Use_InnerRepository_And_Log()
        {
            _innerRepository.Expect(r => r.CountByNew()).Returns(0).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByNew();
        }

        [Fact]
        public void CountByPublishable_Should_Log()
        {
            CountByPublishable();

            log.Verify();
        }

        [Fact]
        public void CountByPublishable_Should_Use_InnerRepository()
        {
            CountByPublishable();

            _innerRepository.Verify();
        }

        private void Add()
        {
            _innerRepository.Expect(r => r.Add(It.IsAny<IStory>())).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.Add(CreateStubStory());
        }

        private void Remove()
        {
            _innerRepository.Expect(r => r.Remove(It.IsAny<IStory>())).Verifiable();
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            _loggingRepository.Remove(CreateStubStory());
        }

        private void FindById(IStory result)
        {
            _innerRepository.Expect(r => r.FindById(It.IsAny<Guid>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindById(Guid.NewGuid());
        }

        private void FindByUniqueName(IStory result)
        {
            _innerRepository.Expect(r => r.FindByUniqueName(It.IsAny<string>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUniqueName("a-dummy-story");
        }

        private void FindByUrl(IStory result)
        {
            _innerRepository.Expect(r => r.FindByUrl(It.IsAny<string>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result == null)
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByUrl("http://story.com");
        }

        private void FindPublished(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindPublished(It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPublished(0, 10);
        }

        private void FindPublishedByCategory(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindPublishedByCategory(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPublishedByCategory(Guid.NewGuid(), 0, 10);
        }

        private void FindUpcoming(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindUpcoming(It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindUpcoming(0, 10);
        }

        private void FindByTag(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindByTag(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindByTag(Guid.NewGuid(), 0, 10);
        }

        private void Search(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.Search(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.Search("query", 0, 10);
        }

        private void FindPostedByUser(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindPostedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPostedByUser(Guid.NewGuid(), 0, 10);
        }

        private void FindPromotedByUser(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindPromotedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPromotedByUser(Guid.NewGuid(), 0, 10);
        }

        private void FindCommentedByUser(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindCommentedByUser(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns((result == null) ? new PagedResult<IStory>() : new PagedResult<IStory>(result, result.Count)).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindCommentedByUser(Guid.NewGuid(), 0, 10);
        }

        private void FindPublishable(ICollection<IStory> result)
        {
            _innerRepository.Expect(r => r.FindPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(result).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            if (result.IsNullOrEmpty())
            {
                log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();
            }

            _loggingRepository.FindPublishable(SystemTime.Now().AddDays(-7), SystemTime.Now().AddHours(-4), 0, 10);
        }

        private void CountByPublished()
        {
            _innerRepository.Expect(r => r.CountByPublished()).Returns(10).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByPublished();
        }

        private void CountByUpcoming()
        {
            _innerRepository.Expect(r => r.CountByUpcoming()).Returns(10).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByUpcoming();
        }

        private void CountByCategory()
        {
            _innerRepository.Expect(r => r.CountByCategory(It.IsAny<Guid>())).Returns(0).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByCategory(Guid.NewGuid());
        }

        private void CountByTag()
        {
            _innerRepository.Expect(r => r.CountByTag(It.IsAny<Guid>())).Returns(0).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByTag(Guid.NewGuid());
        }

        private void CountByNew()
        {
            _innerRepository.Expect(r => r.CountByNew()).Returns(0).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByNew();
        }

        private void CountByPublishable()
        {
            _innerRepository.Expect(r => r.CountByPublishable(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(0).Verifiable();
            log.Expect(l => l.Info(It.IsAny<string>())).Verifiable();

            _loggingRepository.CountByPublishable(SystemTime.Now().AddDays(-7), SystemTime.Now().AddHours(-4));
        }
    }
}