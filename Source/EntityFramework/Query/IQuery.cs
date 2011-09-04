namespace Kigg.Infrastructure.EntityFramework.Query
{
    public interface IQuery<TResult>
    {
        TResult Execute(KiggDbContext context);
    }
}