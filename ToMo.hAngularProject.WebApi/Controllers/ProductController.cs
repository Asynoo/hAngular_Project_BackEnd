using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using hAngular_Project.Dtos.Products;
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

        [HttpGet]
        public ActionResult<ProductAllDto> GetAll()
        {
            var list = _productService.GetProducts()
                .Select(p => new ProductDto {Id = p.Id, Name = p.Name})
                .ToList();
            return Ok(new ProductAllDto {ProductDtos = list});
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return Ok();
        }

        [HttpPut]
        public ActionResult<ProductDto> Create(ProductDto productDto)
        {
            _productService.CreateProduct(new Product{Id = productDto.Id, Name = productDto.Name});
            return Ok(productDto);
        }
        
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