namespace Kigg.Infrastructure.EntityFramework.IntegrationTest
{
    using Xunit;

    public class KiggDbFactoryFixture : IntegrationFixtureBase
    {
        [Fact]
        public void Get_should_return_valid_db_context()
        {
            var dbContext = dbFactory.Get();
            
            Assert.NotNull(dbContext);
            
            Assert.True(dbContext.Database.Exists());
        }
    }
}
