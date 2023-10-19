using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_ProjectAT2.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace WebApplication_ProjectAT2.Controllers
{
    [ApiVersion ("1.0")]
    //[Route ("api/[controller]")]
    //[Route("v{v:apiVersion}/products")]
    [Route("products")]
    [ApiController]
    
    public class FlowerV1Controller : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public FlowerV1Controller(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
        }

        [HttpGet]
        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return _shopContext.Products.ToArray();
        //}
        //public async Task<ActionResult> GetAllProducts()
        //{
        //    return Ok(await _shopContext.Products.ToArrayAsync());
        //}

        //public async Task<ActionResult> GetAllProducts([FromQuery] QueryParameters queryParameters)
        //{
        //    IQueryable<Product> products = _shopContext.Products;

        //    products = products.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);

        //    return Ok(await _shopContext.Products.ToArrayAsync());
        //}

        public async Task<ActionResult> GetAllProducts([FromQuery] ProductParametersQuery queryParameters)
        {
            IQueryable<Product> products = _shopContext.Products;


            if (queryParameters.MinPrice != null)
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value);
            }

            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(p => p.Price <= queryParameters.MaxPrice.Value);
            }
            
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }


            if (!string.IsNullOrEmpty(queryParameters.sortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.sortBy) != null)
                {
                    products = products.OrderByCustom(queryParameters.sortBy, queryParameters.SortOrder);
                }
            }


            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);
            return Ok(await  products.ToArrayAsync());
        }


        
        [HttpGet("{Id}")]
        //public ActionResult GetProduct(int Id)
        //{
        //    var product = _shopContext.Products.Find(Id);

        //    if(product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}
        public async Task<ActionResult> GetProduct(int Id)
        {
            var product = await _shopContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult> PostProduct (Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> PutProduct(int Id, Product product)
        {
            if ( Id != product.Id )
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
                    throw;
            }
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Product>> DeleteProduct (int Id)
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
    [ApiVersion("2.0")]
    //[Route ("api/[controller]")]
    //[Route("v{v:apiVersion}/products")]
    [Route("products")]
    [ApiController]
    
    public class FlowerV2Controller : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public FlowerV2Controller(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductParametersQuery queryParameters)
        {
            IQueryable<Product> products = _shopContext.Products.Where(p => p.IsAvailable == true);


            if (queryParameters.MinPrice != null)
            {
                products = products.Where(p => p.Price >= queryParameters.MinPrice.Value);
            }

            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(p => p.Price <= queryParameters.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(p => p.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }


            if (!string.IsNullOrEmpty(queryParameters.sortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.sortBy) != null)
                {
                    products = products.OrderByCustom(queryParameters.sortBy, queryParameters.SortOrder);
                }
            }


            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);
            return Ok(await products.ToArrayAsync());
        }



        [HttpGet("{Id}")]
        //public ActionResult GetProduct(int Id)
        //{
        //    var product = _shopContext.Products.Find(Id);

        //    if(product == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(product);
        //}
        public async Task<ActionResult> GetProduct(int Id)
        {
            var product = await _shopContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost]
        public async Task<ActionResult> PostProduct(Product product)
        {
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

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
                    throw;
            }
            return NoContent();
        }

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
