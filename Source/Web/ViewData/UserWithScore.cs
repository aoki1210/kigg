namespace Kigg.Web
{
    using DomainObjects;

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