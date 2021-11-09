using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using hAngular_Project.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToMo.hAngularProject.Core.IServices;
using ToMo.hAngularProject.Core.Models;
using Xunit;

namespace ToMo.hAngularProject.WebApi.Test
{
    public class ProductControllerTest
    {
        
        [Fact]
        public void ProductController_IsOfTypeController()
        {
            var service = new Mock<IProductService>();
            var controller = new ProductController(service.Object);
            Assert.IsAssignableFrom<ControllerBase>(controller);
        }
        
        [Fact]
        public void ProductController_UsesApiControllerAttribute()
        {
            var typeInfo = typeof(ProductController).GetTypeInfo();
            var attribute = typeInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType().Name.Equals("ApiControllerAttribute"));
            Assert.NotNull(attribute);
        }
        
        [Fact]
        public void ProductController_UsesRouteAttribute_WithParamApiControllerNameRoute()
        {
            var typeInfo = typeof(ProductController).GetTypeInfo();
            var attribute = typeInfo.GetCustomAttributes().FirstOrDefault(a => a.GetType().Name.Equals("RouteAttribute"));
            Assert.NotNull(attribute);
            var routeAttribute = attribute as RouteAttribute;
            Assert.Equal("api/[controller]", routeAttribute.Template);
        }

        [Fact]
        public void ProductController_HasGetAllMethod()
        {
            var method = typeof(ProductController).GetMethods().FirstOrDefault(m => "GetAll".Equals(m.Name));
            Assert.NotNull(method);
        }
        
        [Fact]
        public void ProductController_HasGetAllMethod_IsPublic()
        {
            var method = typeof(ProductController).GetMethods().FirstOrDefault(m => "GetAll".Equals(m.Name));
            Assert.True(method.IsPublic);
        }

        [Fact]
        public void ProductController_HasGetAllMethod_ReturnsListOfProductsInActionResult()
        {
            var method = typeof(ProductController).GetMethods().FirstOrDefault(m => "GetAll".Equals(m.Name));
            Assert.Equal(typeof(ActionResult<List<Product>>).FullName, method.ReturnType.FullName);
        }

        [Fact]
        public void GetAll_WithNoParams_HasGetHttpAttribute()
        {
            var methodInfo = typeof(ProductController).GetMethods().FirstOrDefault(m => m.Name == "GetAll");
            var attribute =
                methodInfo.CustomAttributes.FirstOrDefault(ca => ca.AttributeType.Name == "HttpGetAttribute");
            Assert.NotNull(attribute);
        }

        [Fact]
        public void ProductController_HasProductService_IsOfTypeControllerBase()
        {
            var service = new Mock<IProductService>();
            var controller = new ProductController(service.Object);
            Assert.IsAssignableFrom<ControllerBase>(controller);
        }
        
        [Fact]
        public void ProductController_WithNullProductRepository_ThrowsExceptionWithMessage()
        {
            var exception = Assert.Throws<InvalidDataException>(() => new ProductController(null));
            Assert.Equal("ProductService Cannot Be Null", exception.Message);
        }

        [Fact]
        public void GetAll_CallsServicesGetProducts_Once()
        {
            var service = new Mock<IProductService>();
            var controller = new ProductController(service.Object);
            controller.GetAll();
            service.Verify(s => s.GetProducts(), Times.Once);
        }
    }
}