namespace Kigg.Domain.Mapping
{
    using AutoMapper;
    using Domain.Entities;
    using ViewModels;

    public class UserRegistrationMapping
    {
        public UserRegistrationMapping()
        {
            Mapper.CreateMap<User, UserRegistrationViewModel>();
        }
    }
}
