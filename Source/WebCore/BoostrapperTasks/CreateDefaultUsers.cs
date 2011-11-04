namespace Kigg.Web.BoostrapperTasks
{
    using System.Collections.Generic;
    
    using MvcExtensions;
    
    using Domain.Entities;
    using Infrastructure;
    using Repository;

    public class CreateDefaultUsers : BootstrapperTask
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly Settings settings;

        public CreateDefaultUsers(IUserRepository userRepository, IUnitOfWork unitOfWork, Settings settings)
        {
            Check.Argument.IsNotNull(userRepository, "userRepository");
            Check.Argument.IsNotNull(unitOfWork, "unitOfWork");
            Check.Argument.IsNotNull(settings, "settings");

            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.settings = settings;
        }

        public override TaskContinuation Execute()
        {
            IEnumerable<User> users = settings.DefaultUsers;

            bool shouldCommit = false;

            foreach (User user in users)
            {
                if (userRepository.FindByUserName(user.UserName) == null)
                {
                    userRepository.Add(user);
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
