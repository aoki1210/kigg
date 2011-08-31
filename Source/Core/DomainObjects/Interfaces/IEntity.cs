namespace Kigg.DomainObjects
{
    using System;

    public interface IEntity
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