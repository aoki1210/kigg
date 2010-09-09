﻿using Xunit;

namespace Kigg.Core.Test
{
    using Infrastructure;
    using Kigg.Test.Infrastructure;

    public class UnitOfWorkFixture : BaseFixture
    {
        [Fact]
        public void Begin_Should_Return_New_Unit_Of_Work()
        {
            Assert.Same(unitOfWork.Object, UnitOfWork.Begin());
        }
    }
}