namespace Kigg.Web
{
    using Domain.Entities;

    public class UserWithScore
    {
        public User User
        {
            get;
            set;
        }

        public decimal Score
        {
            get;
            set;
        }
    }
}