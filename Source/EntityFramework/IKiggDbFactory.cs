namespace Kigg.Infrastructure.EntityFramework
{
    using System;

    public interface IKiggDbFactory : IDisposable
    {
        KiggDbContext Get();
    }
}
