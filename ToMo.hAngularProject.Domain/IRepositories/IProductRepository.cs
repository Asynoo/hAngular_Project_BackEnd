using System.Collections.Generic;
using ToMo.hAngularProject.Core.Models;

namespace ToMo.hAngularProject.Domain.IRepositories
{
    public interface IProductRepository
    {
        List<Product> FindAll();
    }
}