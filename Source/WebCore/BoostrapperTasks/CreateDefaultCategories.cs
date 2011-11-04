namespace Kigg.Web.BoostrapperTasks
{
    using System.Collections.Generic;
    
    using MvcExtensions;
    
    using Domain.Entities;
    using Infrastructure;
    using Repository;

    public class CreateDefaultCategories : BootstrapperTask
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly Settings settings;

        public CreateDefaultCategories(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, Settings settings)
        {
            Check.Argument.IsNotNull(categoryRepository, "categoryRepository");
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            Check.Argument.IsNotNull(settings, "settings");

            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
            this.settings = settings;
        }

        public override TaskContinuation Execute()
        {
            IEnumerable<Category> users = settings.DefaultCategories;

            bool shouldCommit = false;

            foreach (Category user in users)
            {
                if (categoryRepository.FindByName(user.Name) == null)
                {
                    categoryRepository.Add(user);
                    shouldCommit = true;
                }
            }

            if (shouldCommit)
            {
                unitOfWork.Commit();
            }

            return TaskContinuation.Continue;
        }
    }
}
