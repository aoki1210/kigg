namespace Kigg.Services
{
    using System.Linq;
    using System.Collections.Generic;
    using Domain.Entities;
    using Domain.ViewModels;
    
    public class CategoryListResult
    {
        public CategoryListResult(IEnumerable<Category> categories)
            : this(categories.Map<CategoryViewModel>())
        {
        }

        public CategoryListResult(IEnumerable<CategoryViewModel> categories)
        {
            Categories = categories;
            Count = categories.Count();
        }

        public IEnumerable<CategoryViewModel> Categories
        {
            get;
            private set;
        }

        public int Count
        {
            get;
            private set;
        }
    }
}
