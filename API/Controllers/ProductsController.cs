using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductRepository repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await repo.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.AddProduct(product);

            if (await repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }

            return BadRequest("Product could not be created");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return BadRequest("Cannot update this product");

            repo.UpdateProduct(product);

            if (await repo.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Product cannot be updated");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            repo.DeleteProduct(product);

            if (await repo.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Product could not be deleted");
        }

        private bool ProductExists(int id)
        {
            return repo.ProductExists(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetAllBrands()
        {
            return Ok(await repo.GetBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetAllTypes()
        {
            return Ok(await repo.GetBrandsAsync());
        }

    }
}