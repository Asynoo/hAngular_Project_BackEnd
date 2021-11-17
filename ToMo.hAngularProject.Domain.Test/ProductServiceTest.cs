using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using ToMo.hAngularProject.Core.IServices;
using ToMo.hAngularProject.Core.Models;
using ToMo.hAngularProject.Domain.IRepositories;
using ToMo.hAngularProject.Domain.Services;
using Xunit;

namespace ToMo.hAngularProject.Domain.Test
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mock;
        private readonly ProductService _service;
        private readonly List<Product> _expected;

        public ProductServiceTest()
        {
            _mock = new Mock<IProductRepository>();
            _service = new ProductService(_mock.Object);
            _expected = new List<Product>
            {
                new Product{ Id = 1, Name = "Item1"},
                new Product{ Id = 2, Name = "Item2"}
            };
        }

        [Fact]
        public void ProductService_IsIProductService()
        {
            Assert.True(_service is IProductService);
        }

        [Fact]
        public void ProductService_WithNullProductRepository_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(() => new ProductService(null));
        }
        
        [Fact]
        public void ProductService_WithNullProductRepository_ThrowsExceptionWithMessage()
        {
            var exception = Assert.Throws<InvalidDataException>(() => new ProductService(null));
            Assert.Equal("ProductRepository Cannot Be Null", exception.Message);
        }

        [Fact]
        public void GetProducts_CallsProductRepositoriesFindAll_ExactlyOnce()
        {
            _service.GetProducts();
            _mock.Verify(r => r.FindAll(), Times.Once);
        }
        
        [Fact]
        public void GetProducts_NoFilter_ReturnsListOfAllProducts()
        {
            _mock.Setup(r => r.FindAll()).Returns(_expected);
            var actual = _service.GetProducts();
            Assert.Equal(_expected, actual);
        }

        [Fact]
        public void CreateProduct_CallsAddProductExactlyOnce()
        {
            var product = new Product();
            _service.CreateProduct(product);
            _mock.Verify(r => r.AddProduct(product), Times.Once);
        }
        
        [Fact]
        public void CreateProduct_NoParam_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _service.CreateProduct(null));
            Assert.Equal("Product cannot be null",ex.Message);
        }
    }
}