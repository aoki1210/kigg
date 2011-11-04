namespace Kigg.Domain.ViewModels
{
    using System;

    using Domain.Entities;

    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            
        }
        
        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            UniqueName = category.UniqueName;
            CreatedAt = category.CreatedAt;
        }
        
        public long Id { get; private set; }

        public string Name { get; private set; }

        public string UniqueName { get; private set; }

        public DateTime CreatedAt { get; private set; }

    }
}
