namespace Kigg.Services
{
    using Repository;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        
        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public CategoryListResult GetCategoryList()
        {
            var categories = categoryRepository.FindAll();

            return new CategoryListResult(categories);
        }
    }
}
