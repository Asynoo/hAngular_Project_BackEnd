using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using ToMo.hAngularProject.DataAccess.Entities;
using Xunit;

namespace ToMo.hAngularProject.DataAccess.Test
{
    public class MainDbContextTest
    {
        
        [Fact]
        public void DbContext_WithDbContextOptions_IsAvailable()
        {
            var mockedDbContext = Create.MockedDbContextFor<MainDbContext>();
            Assert.NotNull(mockedDbContext);
        }
        
        [Fact]
        public void DbContext_DbSets_MustHaveDbSetWithTypeProductEntity()
        {
            var mockedDbContext = Create.MockedDbContextFor<MainDbContext>();
            Assert.True(mockedDbContext.Products is DbSet<ProductEntity>);
        }

    }
}