namespace Kigg.Domain.Mapping
{
    using AutoMapper;
    using Domain.Entities;
    using ViewModels;

    public class CategoryMapping
    {
        public CategoryMapping()
        {
            Mapper.CreateMap<Category, CategoryModel>();
        }
    }
}
