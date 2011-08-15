namespace Kigg.Service
{
    using System.Diagnostics;

    using DomainObjects;
    using Infrastructure;

    public class UserActivateEventArgs
    {
        [DebuggerStepThrough]
        public UserActivateEventArgs(User user)
        {
            User = user;
        }

        public User User
        {
            get;
            private set;
        }
    }

    public class UserActivateEvent : BaseEvent<UserActivateEventArgs>
    {
    }
}