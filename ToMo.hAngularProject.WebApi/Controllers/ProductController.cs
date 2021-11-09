using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToMo.hAngularProject.Core.IServices;
using ToMo.hAngularProject.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hAngular_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            if (productService == null)
            {
                throw new InvalidDataException("ProductService Cannot Be Null");
            }

            _productService = productService;
        }

        [HttpGet]
        public ActionResult<List<Product>> GetAll()
        {
            return _productService.GetProducts();
        }
    }
}