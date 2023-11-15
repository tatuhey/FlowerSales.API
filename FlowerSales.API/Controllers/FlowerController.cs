using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlowerSales.API.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace FlowerSales.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("products")]
    [ApiController]
    public class FlowerV1Controller : ControllerBase
    {
        private readonly ShopContext _shopContext;

        // Constructor to initialize the database context
        public FlowerV1Controller(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
        }

        // GET: api/products
        // Retrieves all products
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            return _shopContext.Products.ToArray();
        }

        // GET: api/products/{Id}
        // Retrieves a specific product by ID
        [HttpGet("{Id}")]
        public ActionResult GetProduct(int Id)
        {
            var product = _shopContext.Products.Find(Id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }


    [ApiVersion("2.0")]
    [Route("products")]
    [ApiController]
    public class FlowerV2Controller : ControllerBase
    {
        private readonly ShopContext _shopContext;

        // Constructor to initialize the database context
        public FlowerV2Controller(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
        }

        // GET: api/products
        // Retrieves all products with optional query parameters for filtering and sorting
        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductParametersQuery queryParameters)
        {
            IQueryable<Product> products = _shopContext.Products.Where(p => p.IsAvailable == true);

            // Filter by price range
            if (queryParameters.MinPrice != null)
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value);
            }
            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(p => p.Price <= queryParameters.MaxPrice.Value);
            }

            // Filter by name
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            // Sorting products if a valid sort property
            if (!string.IsNullOrEmpty(queryParameters.sortBy) && typeof(Product).GetProperty(queryParameters.sortBy) != null)
            {
                products = products.OrderByCustom(queryParameters.sortBy, queryParameters.SortOrder);
            }

            // Pagination
            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);
            return Ok(await products.ToArrayAsync());
        }

        // GET: api/products/{Id}
        // Retrieves a specific product by ID
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProduct(int Id)
        {
            var product = await _shopContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/products
        // Adds a new product
        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // PUT: api/products/{Id}
        // Updates an existing product
        [HttpPut("{Id}")]
        public async Task<ActionResult> PutProduct(int Id, Product product)
        {
            if (Id != product.Id)
            {
                return BadRequest();
            }

            _shopContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _shopContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_shopContext.Products.Any(p => p.Id == Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/products/{Id}
        // Deletes a specific product
        [HttpDelete("{Id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int Id)
        {
            var product = await _shopContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            _shopContext.Products.Remove(product);
            await _shopContext.SaveChangesAsync();
            return product;
        }
    }
}