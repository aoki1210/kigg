namespace Kigg.Domain.Entities
{
    using System;

    [Flags]
    public enum Role
    {
        User = 0,
        Bot = 1,
        Moderator = 2,
        Administrator = 4
    }
}