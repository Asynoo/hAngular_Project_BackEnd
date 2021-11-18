using System.Collections.Generic;
using ToMo.hAngularProject.Core.Models;

namespace ToMo.hAngularProject.Domain.IRepositories
{
    public interface IProductRepository
    {
        List<Product> FindAll();
        Product AddProduct(Product product);
        void RemoveProduct(int id);
        void UpdateProduct(Product product);
        Product FindProduct(int productId);
    }
}