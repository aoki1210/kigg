namespace Kigg.Web.BoostrapperTasks
{
    using MvcExtensions;

    public class NoopTask : BootstrapperTask
    {
        public override TaskContinuation Execute()
        {
            return TaskContinuation.Continue;
        }
    }
}
