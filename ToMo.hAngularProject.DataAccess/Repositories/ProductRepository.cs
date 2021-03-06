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
        public Product AddProduct(Product product)
        {
            var pe = new ProductEntity{Name = product.Name};
            var createdEntity = _ctx.Products.Add(pe);
            _ctx.SaveChanges();
            return new Product {Id = createdEntity.Entity.Id,Name = createdEntity.Entity.Name};
        }

        public void RemoveProduct(int productId)
        {
            _ctx.Products.Remove(_ctx.Products.FirstOrDefault(entity => entity.Id == productId));
            _ctx.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            if (!_ctx.Products.Any(entity => entity.Id == product.Id)) return;
            _ctx.Products.Update(new ProductEntity{Id = product.Id,Name = product.Name});
            _ctx.SaveChanges();
        }

        public Product FindProduct(int productId)
        {
            var product = _ctx.Products.FirstOrDefault(entity => entity.Id == productId);
            return product != null ? new Product {Id = product.Id, Name = product.Name} : null;
        }
    }
}