using System;
using System.Collections.Generic;
using System.IO;
using ToMo.hAngularProject.Core.IServices;
using ToMo.hAngularProject.Core.Models;
using ToMo.hAngularProject.Domain.IRepositories;

namespace ToMo.hAngularProject.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new InvalidDataException("ProductRepository Cannot Be Null");
        }

        public List<Product> GetProducts()
        {
            return _productRepository.FindAll();

        }

        public void CreateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentException("Product cannot be null");
            _productRepository.AddProduct(product);
        }

        public void DeleteProduct(int productId)
        {
            _productRepository.RemoveProduct(productId);
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentException("Product cannot be null");
            _productRepository.UpdateProduct(product);
        }
    }
}