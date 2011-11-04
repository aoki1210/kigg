namespace Kigg.Service
{
    using System.Diagnostics;

    using Domain.Entities;
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