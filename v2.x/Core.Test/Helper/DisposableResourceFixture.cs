using System;
using Xunit;

namespace Kigg.Core.Test
{
    public class DisposableResourceFixture : IDisposable
    {
        private DisposableResourceTestDouble _resource;

        public DisposableResourceFixture()
        {
            _resource = new DisposableResourceTestDouble();
        }

        public void Dispose()
        {
            _resource = null;
        }

        [Fact]
        public void Dispose_Should_Call_Protected_Dispose()
        {
            _resource.Dispose();

            Assert.True(_resource.IsDisposed);
        }

        private class DisposableResourceTestDouble  : DisposableResource
        {
            public bool IsDisposed;

            protected override void Dispose(bool disposing)
            {
                IsDisposed = true;
                base.Dispose(disposing);
            }
        }
    }
}