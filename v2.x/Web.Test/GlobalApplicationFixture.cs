using System;
using System.Web;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class GlobalApplicationFixture : BaseFixture
    {
        [Fact]
        public void OnStart_Should_Not_Throw_Exception()
        {
            GlobalApplication.OnStart();
        }

        [Fact]
        public void OnError_Should_Log_Exception()
        {
            var exception = new Exception("An exception", new InvalidOperationException());
            var httpServerUtility = new Mock<HttpServerUtilityBase>();

            httpServerUtility.Expect(s => s.GetLastError()).Returns(exception);

            log.Expect(l => l.Exception(It.IsAny<Exception>())).Verifiable();

            GlobalApplication.OnError(httpServerUtility.Object);

            log.Verify();
        }

        [Fact]
        public void OnError_Should_Not_Log_Http404_Exception()
        {
            var exception = new Exception("An exception", new HttpException(404, "Not Found"));
            var httpServerUtility = new Mock<HttpServerUtilityBase>();

            httpServerUtility.Expect(s => s.GetLastError()).Returns(exception);

            bool logged = false;

            log.Expect(l => l.Exception(It.IsAny<Exception>())).Callback(() => logged = true);

            GlobalApplication.OnError(httpServerUtility.Object);

            Assert.False(logged);
        }

        [Fact]
        public void OnEnd_Should_Log_Warning()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            GlobalApplication.OnEnd();

            log.Verify();
        }
    }
}