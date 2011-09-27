namespace Kigg.DomainObjects
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