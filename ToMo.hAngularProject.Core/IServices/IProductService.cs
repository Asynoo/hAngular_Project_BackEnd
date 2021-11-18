using System.Collections.Generic;
using ToMo.hAngularProject.Core.Models;

namespace ToMo.hAngularProject.Core.IServices
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product GetProduct(int productId);
        Product CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);
        
        
    }
}