namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;

    using DomainObjects;

    public interface IUserRepository : IEntityRepository<User>
    {
        User FindByUserName(string userName);

        User FindByEmail(string email);

        decimal FindScoreById(long id, DateTime startTimestamp, DateTime endTimestamp);

        PagedResult<User> FindTop(DateTime startTimestamp, DateTime endTimestamp, int start, int max);

        PagedResult<User> FindAll(int start, int max);

        ICollection<string> FindIPAddresses(long id);
    }
}