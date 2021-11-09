using System.Collections.Generic;
using ToMo.hAngularProject.Core.Models;

namespace ToMo.hAngularProject.Core.IServices
{
    public interface IProductService
    {
        List<Product> GetProducts();
    }
}