using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_ProjectAT2.Models;
using System.Collections.Generic;

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
        public IEnumerable<Product> GetAllProducts()
        {
            return _shopContext.Products.ToArray();
        }

        
        [HttpGet("{Id}")]
        public ActionResult GetProduct(int Id)
        {
            var product = _shopContext.Products.Find(Id);

            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

    }
}
