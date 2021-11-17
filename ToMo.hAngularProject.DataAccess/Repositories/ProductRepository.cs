using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToMo.hAngularProject.Core.Models;
using ToMo.hAngularProject.DataAccess.Entities;
using ToMo.hAngularProject.Domain.IRepositories;

namespace ToMo.hAngularProject.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MainDbContext _ctx;

        public ProductRepository(MainDbContext ctx)
        {
            if (ctx == null) throw new InvalidDataException("Product Repository Must have a DBContext");
            _ctx = ctx;
        }

        public List<Product> FindAll()
        {
            return _ctx.Products.Select(pe => new Product {Id = pe.Id, Name = pe.Name}).ToList();
        }
        public void AddProduct(Product product)
        {
            var pe = new ProductEntity{Name = product.Name};
            _ctx.Products.Add(pe);
            _ctx.SaveChanges();
        }

        public void RemoveProduct(int id)
        {
            _ctx.Products.Remove(_ctx.Products.FirstOrDefault(entity => entity.Id == id));
            _ctx.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            _ctx.Products.Update(new ProductEntity());
            _ctx.SaveChanges();
        }
    }
}