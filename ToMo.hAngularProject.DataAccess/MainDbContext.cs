using Microsoft.EntityFrameworkCore;
using ToMo.hAngularProject.DataAccess.Entities;

namespace ToMo.hAngularProject.DataAccess
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options): base(options)
        {
            
        }

        public virtual DbSet<ProductEntity> Products { get; set; }
    }
}