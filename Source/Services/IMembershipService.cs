namespace Kigg.Services
{
    using Kigg.Domain.Entities;
    using Kigg.Domain.ViewModels;

    public interface IMembershipService
    {
        CreateUserResult CreateUser(UserRegistrationModel user);
    }
}