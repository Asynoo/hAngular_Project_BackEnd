﻿using System.Collections.Generic;
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
    }
}