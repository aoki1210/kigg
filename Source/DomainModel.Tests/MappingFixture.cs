using Kigg.Domain.ViewModels;

namespace Kigg.Domain.Tests
{
    using Xunit;

    using FizzWare.NBuilder;
    using AutoMapper;
    
    using Mapping;
    using Domain.Entities;
    using Domain.ViewModels;

    public class MappingFixture
    {
        [Fact]
        public void CategoryMapping_mapping_assert_does_not_throw_exception()
        {
            new CategoryMapping();

            Mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void CategoryViewModel_map_returns_valid_category_view_model()
        {
            new CategoryMapping();
            
            var domainObject = Builder<Category>.CreateNew().Build();

            var viewModel = Mapper.Map<CategoryViewModel>(domainObject);

            Assert.Equal(domainObject.Id, viewModel.Id);
            Assert.Equal(domainObject.CreatedAt, viewModel.CreatedAt);
            Assert.Equal(domainObject.Name, viewModel.Name);
            Assert.Equal(domainObject.UniqueName, viewModel.UniqueName);
        }

        [Fact]
        public void EnumerableExtensions_map_returns_valid_category_view_model_list()
        {
            new CategoryMapping();

            var domainObjects = Builder<Category>.CreateListOfSize(5).Build();

            var viewModels = domainObjects.Map<CategoryViewModel>();

            Assert.NotEmpty(viewModels);
        }
    }
}
