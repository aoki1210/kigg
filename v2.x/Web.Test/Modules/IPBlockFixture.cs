using System;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Kigg.Test.Infrastructure;

    public class IPBlockFixture : BaseFixture
    {
        private readonly HttpContextMock _httpContext;
        private readonly BaseHttpModule _module;

        public IPBlockFixture()
        {
            const string BlockedIP = "192.168.0.1";

            file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(string.Empty);

            var collection = new Mock<IBlockedIPCollection>();

            collection.Expect(c => c.Contains(BlockedIP)).Returns(true);
            resolver.Expect(r => r.Resolve<IBlockedIPCollection>()).Returns(collection.Object);

            _httpContext = MvcTestHelper.GetHttpContext();

            _httpContext.HttpRequest.ExpectGet(r => r.UserHostAddress).Returns(BlockedIP);
            _httpContext.HttpRequest.ExpectGet(r => r.Url).Returns(new Uri("http://dotnetshoutout.com/Upcoming"));

            _module = new IPBlock();
        }

        [Fact]
        public void OnBeginRequest_Should_Block_Ip_If_Ip_Exists_In_BlockedIpCollection()
        {
            _httpContext.Expect(c => c.RewritePath(It.IsAny<string>())).Verifiable();
            _module.OnBeginRequest(_httpContext.Object);

            _httpContext.Verify();
        }

        [Fact]
        public void OnBeginRequest_Should_Warn_In_Log_When_Ip_Address_Is_Blocked()
        {
            log.Expect(l => l.Warning(It.IsAny<string>())).Verifiable();

            _module.OnBeginRequest(_httpContext.Object);

            log.Verify();
        }
    }
}