using System.IO;
using System.Linq;
using hAngular_Project.Dtos.Products;
using hAngular_Project.PolicyHandlers;
using Microsoft.AspNetCore.Authorization;
using ToMo.hAngularProject.Core.IServices;
using ToMo.hAngularProject.Core.Models;
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

        
        [Authorize(Policy=nameof(CanReadProductsHandler))]
        [HttpGet]
        public ActionResult<ProductAllDto> Get()
        {
            var list = _productService.GetProducts()
                .Select(p => new ProductDto {Id = p.Id, Name = p.Name})
                .ToList();
            return Ok(list);
        }
        
        [Authorize(Policy=nameof(CanReadProductsHandler))]
        [HttpGet("{id:int}")]
        public ActionResult<ProductAllDto> Get(int id)
        {
            var product = _productService.GetProduct(id);
            if (product == null) return NotFound();
            return Ok(new ProductDto {Id = product.Id, Name = product.Name});
        }

        [Authorize(Policy=nameof(CanRemoveProductsHandler))]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return Ok();
        }

        [Authorize(Policy=nameof(CanWriteProductsHandler))]
        [HttpPost]
        public ActionResult<ProductDto> Create(ProductDto productDto)
        {
            var product = _productService.CreateProduct(new Product{Id = productDto.Id, Name = productDto.Name});
            return StatusCode(201,productDto);
        }
        
        [Authorize(Policy=nameof(CanEditProductsHandler))]
        [HttpPut("{id:int}")]
        public ActionResult<ProductDto> Put(int id, ProductDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Ids dont match");
            }
            _productService.UpdateProduct(new Product
            {
                Id = dto.Id,
                Name = dto.Name
            });
            return Ok(dto);
        }
    }
}