using System;
using System.Linq;
using System.Data;
using System.Transactions;

using Xunit;

namespace Kigg.Infrastructure.EF.IntegrationTest
{
    using Kigg.EF.Repository;
    using Kigg.EF.DomainObjects;
    
    public class CategoryRepositoryFixture : BaseIntegrationFixture
    {
        private readonly CategoryRepository _categoryRepository;
        
        public CategoryRepositoryFixture()
        {
            _categoryRepository = new CategoryRepository(_database);
        }

        [Fact]
        public void Constructor_With_Factory_Does_Not_Throw_Exception()
        {
            Assert.DoesNotThrow(() => new CategoryRepository(_dbFactory));
        }

        [Fact]
        public void Add_And_Presist_Changes_Should_Succeed()
        {
            using (new TransactionScope())
            {
                var category = (Category) _domainFactory.CreateCategory("Dummy Category");
                _categoryRepository.Add(category);
                Assert.Equal(EntityState.Added, category.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Unchanged, category.EntityState);
            }
        }

        [Fact]
        public void Add_Should_Throw_Exception_When_Specified_Name_Already_Exists()
        {
            var category = _database.CategoryDataSource.First();

            Assert.Throws<ArgumentException>(() => _categoryRepository.Add(_domainFactory.CreateCategory(category.Name)));
        }

        [Fact]
        public void Remove_And_Presist_Changes_Should_Succeed()
        {
            var category = _database.CategoryDataSource.First();
            using (new TransactionScope())
            {
                _categoryRepository.Remove(category);
                Assert.Equal(EntityState.Deleted, category.EntityState);
                _database.SubmitChanges();
                Assert.Equal(EntityState.Detached, category.EntityState);
            }
        }

        [Fact]
        public void FindById_Should_Return_Correct_Category()
        {
            var existingCategory = _database.CategoryDataSource.First();
            var category = _categoryRepository.FindById(existingCategory.Id);
            Assert.Equal(existingCategory.Id, category.Id);
        }

        [Fact]
        public void FindByUniqueName_Should_Return_Correct_Category()
        {
            var existingCategory = _database.CategoryDataSource.First();
            var category = _categoryRepository.FindByUniqueName(existingCategory.UniqueName);
            Assert.Equal(existingCategory.UniqueName, category.UniqueName);
        }

        [Fact]
        public void FindAll_Should_Return_All_Category()
        {
            int count = _database.CategoryDataSource.Count();

            var result = _categoryRepository.FindAll();

            Assert.Equal(count, result.Count);
        }
    }
}

    