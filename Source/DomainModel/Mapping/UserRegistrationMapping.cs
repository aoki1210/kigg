namespace Kigg.Domain.Mapping
{
    using AutoMapper;
    using Domain.Entities;
    using Domain.ViewModels;

    public class UserRegistrationMapping
    {
        public UserRegistrationMapping()
        {
            Mapper.CreateMap<UserRegistrationModel, User>();
        }
    }
}
