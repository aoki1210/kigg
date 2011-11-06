namespace Kigg.Web.Controllers
{
    using System.Web.Mvc;
    using Services;

    public class CategoryController : KiggControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            Check.Argument.IsNotNull(categoryService, "categoryService");

            this.categoryService = categoryService;
        }

        [ValidateInput(false)]
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var categories = categoryService.GetCategoryList();
            return View("_menu",categories);
        }
    }
}
