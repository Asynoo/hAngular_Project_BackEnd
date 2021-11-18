using System.Collections.Generic;
using ToMo.hAngularProject.Core.Models;

namespace ToMo.hAngularProject.Core.IServices
{
    public interface IProductService
    {
        List<Product> GetProducts();
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);
        
        
    }
}