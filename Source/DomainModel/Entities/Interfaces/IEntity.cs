namespace Kigg.Domain.Entities
{
    using System;

    public interface IEntity : IDomainObject
    {
        long Id
        {
            get;
        }

        DateTime CreatedAt
        {
            get;
        }
    }
}