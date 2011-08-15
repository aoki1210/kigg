using System;
using System.Collections.Generic;

using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using DomainObjects;
    using Repository;

    public class CachingTagRepositoryFixture : DecoratedRepositoryFixture
    {
        private readonly Mock<ITagRepository> _innerRepository;

        public CachingTagRepositoryFixture()
        {
            _innerRepository = new Mock<ITagRepository>();

            FindByUsage();
        }

        [Fact]
        public void FindByUsage_Should_Cache()
        {
            cache.Verify();
        }

        [Fact]
        public void FindByUsage_Should_Use_InnerRepository()
        {
            _innerRepository.Verify();
        }

        private void FindByUsage()
        {
            var repository = new CachingTagRepository(_innerRepository.Object, 3);

            cache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<ICollection<Tag>>(), It.IsAny<DateTime>())).Verifiable();

            _innerRepository.Setup(r => r.FindByUsage(It.IsAny<int>())).Returns(new List<Tag> { CreateStubTag() }).Verifiable();

            repository.FindByUsage(1);
        }
    }
}