using Moq;
using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;

    public class StartBackgroundTasksFixture
    {
        private readonly Mock<IBackgroundTask> _task1;
        private readonly Mock<IBackgroundTask> _task2;

        private readonly StartBackgroundTasks _startTasks;

        public StartBackgroundTasksFixture()
        {
            _task1 = new Mock<IBackgroundTask>();
            _task2 = new Mock<IBackgroundTask>();

            _startTasks = new StartBackgroundTasks(new[] { _task1.Object, _task2.Object });
        }

        [Fact]
        public void Exceute_Should_Start_Tasks()
        {
            _task1.Expect(t => t.Start()).Verifiable();
            _task2.Expect(t => t.Start()).Verifiable();

            _startTasks.Execute();

            _task1.Verify();
            _task2.Verify();
        }
    }
}