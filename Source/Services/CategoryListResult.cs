namespace Kigg.Services
{
    using System.Linq;
    using System.Collections.Generic;
    using Domain.Entities;
    using Domain.ViewModels;
    
    public class CategoryListResult
    {
        public CategoryListResult(IEnumerable<Category> categories)
            : this(categories.Map<CategoryModel>())
        {
        }

        public CategoryListResult(IEnumerable<CategoryModel> categories)
        {
            Categories = categories;
            Count = categories.Count();
        }

        public IEnumerable<CategoryModel> Categories
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
