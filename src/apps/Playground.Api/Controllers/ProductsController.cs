using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playground.Api.Models;

namespace Playground.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "product")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> Products = new()
        {
            new Product {ProductId = 1,ProductName = "book",ProductCategory="Study",ProductDescription="Finance Book"}
        };

        //GET
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(Products);
        }
/*
        //POST
        [HttpPost]
        public IActionResult AddProducts(int id, [FromBody] Product products)
        {
            Products.Add(products);
            return CreatedAtAction(nameof(GetProducts), new { id = products.ProductId }, products);
        }

        //PUT
        [HttpPut("{id}")]
        public IActionResult UpdateProducts(int id, [FromBody]Product product)
        {
            Product? Product = Products.FirstOrDefault(p => p.ProductId == id);
            if (Products == null) return NotFound();
            Product.ProductName = product.ProductName;
            Product.ProductId = product.ProductId;
            Product.ProductDescription = product.ProductDescription;

            return Ok(product);
        }

        //DELETE
        public IActionResult DeleteProducts(int id)
        {
            Product products = Products.FirstOrDefault(b => b.ProductId == id);
            if (Products == null) return NotFound();
            Products.Remove(products);
            return NoContent();
        }*/
    }
}
