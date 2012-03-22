namespace Kigg.Infrastructure.EntityFramework
{
    using System.Diagnostics;

    using Infrastructure;

    public class UnitOfWork : Disposable, IUnitOfWork
    {
        private readonly IKiggDbFactory dbFactory;
        private KiggDbContext context;

        public UnitOfWork(IKiggDbFactory dbFactory)
        {
            Check.Argument.IsNotNull(dbFactory, "dbFactory");

            this.dbFactory = dbFactory;
        }

        protected KiggDbContext Context
        {
            [DebuggerStepThrough]
            get
            {
                return context ?? (context = dbFactory.Get());
            }
        }

        public void Commit()
        {
            Context.Commit();
        }

        protected override void DisposeCore()
        {
            //dbFactory.Dispose();
        }
    }
}