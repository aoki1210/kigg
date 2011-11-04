namespace Kigg.DomainModel.Mapping
{
    using AutoMapper;

    using Domain.Entities;
    using Domain.ViewModels;

    public class CategoryMapping
    {
        public CategoryMapping()
        {
            Mapper.CreateMap<Category, CategoryViewModel>();
        }
    }
}
