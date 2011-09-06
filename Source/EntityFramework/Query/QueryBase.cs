namespace Kigg.Infrastructure.EntityFramework.Query
{
    public abstract class QueryBase<TResult> : IQuery<TResult>
    {
        protected readonly KiggDbContext context;
        protected QueryBase(KiggDbContext context, bool useCompiled)
        {
            Check.Argument.IsNotNull(context, "context");

            this.context = context;
            UseCompiled = useCompiled;
        }

        protected QueryBase(KiggDbContext context)
            : this(context, true)
        {
            
        }

        protected bool UseCompiled
        {
            get;
            private set;
        }

        public abstract TResult Execute(); 
    }
}