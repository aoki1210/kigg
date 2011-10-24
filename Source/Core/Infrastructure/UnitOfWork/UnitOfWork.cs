namespace Kigg.Infrastructure
{
    using System;
    using System.Diagnostics;

    [Obsolete("Not applicable any more")]
    public static class UnitOfWork
    {
        [DebuggerStepThrough]
        public static IUnitOfWork Begin()
        {
            return IoC.Resolve<IUnitOfWork>();
        }
    }
}