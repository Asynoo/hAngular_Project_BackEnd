using ToMo.hAngularProject.DataAccess.Entities;

namespace ToMo.hAngularProject.DataAccess
{
    public class DbSeeder
    {
        private readonly MainDbContext _ctx;

        public DbSeeder(MainDbContext ctx)
        {
            _ctx = ctx;
        }

        public void SeedDevelopment()
        {
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();
            _ctx.Products.Add(new ProductEntity {Name = "Item1"});
            _ctx.Products.Add(new ProductEntity {Name = "Item2"});
            _ctx.Products.Add(new ProductEntity {Name = "Item3"});
            _ctx.SaveChanges();
        }
    }
}