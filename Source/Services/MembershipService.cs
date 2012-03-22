
namespace Kigg.Services
{
    using System;
    using AutoMapper;
    using Kigg.Domain.Entities;
    using Kigg.Domain.ViewModels;
    using Kigg.Infrastructure;
    using Kigg.Repository;

    public class MembershipService : IMembershipService
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public MembershipService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public CreateUserResult CreateUser(UserRegistrationModel userModel)
        {
            try
            {
                var userEntity = Mapper.Map<User>(userModel);
                userEntity.IsActive = true;
                userEntity.CreatedAt = SystemTime.Now;
                userEntity.LastActivityAt = SystemTime.Now;

                userRepository.Add(userEntity);

                unitOfWork.Commit();

                return CreateUserResult.Succeeded;
            }
            catch (ArgumentException)
            {
                //TODO Handle User Name or Email already exists
                return CreateUserResult.Failed;
            }
        }
    }
}
