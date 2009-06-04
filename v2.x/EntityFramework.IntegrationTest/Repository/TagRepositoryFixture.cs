using System;
using System.Linq;
using System.Data;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.DomainObjects;
    using Kigg.EF.Repository;

    public class TagRepositoryFixture : BaseIntegrationFixture
    {
        private readonly TagRepository _tagRepository;

        public TagRepositoryFixture()
        {   
            _tagRepository = new TagRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new TagRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var tag = (Tag)_domainFactory.CreateTag("Dummy Category");
                _tagRepository.Add(tag);
                Assert.Equal(EntityState.Added, tag.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, tag.EntityState);
            }
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            var tag = _database.TagDataSource.First();

            Assert.Throws<ArgumentException>(() => _tagRepository.Add(_domainFactory.CreateTag(tag.Name)));
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            var tag = _database.TagDataSource.First();
            using (new TransactionScope())
            {
                _tagRepository.Remove(tag);
                Assert.Equal(EntityState.Deleted, tag.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, tag.EntityState);
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_Tag()
        {
            var id = GetAnyExistingTag().Id;
            
            var tag = _tagRepository.FindById(id);

            Assert.Equal(id, tag.Id);
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Tag()
        {
            var uniqueName = GetAnyExistingTag().UniqueName;
            
            var tag = _tagRepository.FindByUniqueName(uniqueName);

            Assert.Equal(uniqueName, tag.UniqueName);
        }

        [Fact]
        public void FindByName_Should_Return_Correct_Tag()
        {
            var name = GetAnyExistingTag().Name;

            var tag = _tagRepository.FindByName(name);

            Assert.Equal(name, tag.Name);
        }

        [Fact]
        public void FindMatching_Should_Return_Correct_Tags()
        {
            using (new TransactionScope())
            {
                _tagRepository.Add(_domainFactory.CreateTag("UniqueDemo 01"));
                _tagRepository.Add(_domainFactory.CreateTag("UniqueDemo 02"));
                _tagRepository.Add(_domainFactory.CreateTag("UniqueDemo 03"));
                _database.SubmitChanges();
                var tags = _tagRepository.FindMatching("UniqueDemo", 10);
                Assert.Equal(3, tags.Count);
            }
        }

        [Fact]
        public void FindByUsage_Should_Return_Top_Tags()
        {
            var count = _database.TagDataSource
                .Where(t => t.StoriesInternal.Any())
                .OrderByDescending(t => t.StoriesInternal.Count(st => st.ApprovedAt != null))
                .ThenBy(t => t.Name)
                .Take(10).Count();

            var result = _tagRepository.FindByUsage(10);

            Assert.Equal(count, result.Count);
        }

        [Fact]
        public void FindAll_Should_Return_All_Tag()
        {
            var count = _database.TagDataSource.Count();

            var result = _tagRepository.FindAll();

            Assert.Equal(count, result.Count);
        }

        private Tag GetAnyExistingTag()
        {
            return _database.TagDataSource.First();
        }
    }
}
