namespace Kigg.Web
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Domain.Entities;
    using Repository;

    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");

            _categoryRepository = categoryRepository;
        }

        private IEnumerable<Category> Categories
        {
            get { return _categoryRepository.FindAll(); }
        }

        [ValidateInput(false)]
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return View(Categories);
        }

        [ChildActionOnly]
        public ActionResult RadioButtonList()
        {
            return View(Categories);
        }
    }
}