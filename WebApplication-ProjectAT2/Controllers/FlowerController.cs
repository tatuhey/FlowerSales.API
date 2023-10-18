using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_ProjectAT2.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace WebApplication_ProjectAT2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowerController : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public FlowerController(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated();
        }

        [HttpGet]
        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return _shopContext.Products.ToArray();
        //}
        public async Task<ActionResult> GetAllProducts()
        {
            return Ok(await _shopContext.Products.ToArrayAsync());
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

    }
}
