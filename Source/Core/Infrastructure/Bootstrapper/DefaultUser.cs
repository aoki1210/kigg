namespace Kigg.Infrastructure
{
    using Domain.Entities;

    public class DefaultUser
    {
        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public Role Role
        {
            get;
            set;
        }
    }
}