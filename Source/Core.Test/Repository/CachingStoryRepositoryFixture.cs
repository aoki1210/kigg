using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Domain.Entities;
    using Repository;

    public class CachingStoryRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<IStoryRepository> _innerRepository;
        private readonly CachingStoryRepository _repository;

        public CachingStoryRepositoryFixture()
        {
            _innerRepository = new Mock<IStoryRepository>();
            _repository = new CachingStoryRepository(_innerRepository.Object, 50, 1, 30, .5f);
        }

        public override void Dispose()
        {
            _innerRepository.Verify();
            cache.Verify();
        }

        [Fact]
        public void FindPublished_Should_Cache()
        {
            FindPublished();

            cache.Verify();
        }

        [Fact]
        public void FindPublished_Should_Use_InnerRepository()
        {
            FindPublished();

            _innerRepository.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Cache()
        {
            FindPublished();

            cache.Verify();
        }

        [Fact]
        public void FindUpcoming_Should_Use_InnerRepository()
        {
            FindUpcoming();

            _innerRepository.Verify();
        }

        public void FindUpcoming()
        {
            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<PagedResult<Story>>(), It.IsAny<DateTime>())).Verifiable();

            _innerRepository.Setup(r => r.FindUpcoming(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<Story>(new List<Story> { CreateStubStory() }, 1)).Verifiable();

            _repository.FindUpcoming(0, 10);
        }

        private void FindPublished()
        {
            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<PagedResult<Story>>(), It.IsAny<DateTime>())).Verifiable();

            _innerRepository.Setup(r => r.FindPublished(It.IsAny<int>(), It.IsAny<int>())).Returns(new PagedResult<Story>(new List<Story> { CreateStubStory() }, 1)).Verifiable();

            _repository.FindPublished(0, 10);
        }
    }
}