using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // This is a placeholder for actual product retrieval logic.
            // In a real application, you would typically retrieve products from a database.
            var products = new[]
            {
                new { Id = 1, Name = "Product 1", Price = 10.99 },
                new { Id = 2, Name = "Product 2", Price = 15.49 },
                new { Id = 3, Name = "Product 3", Price = 7.99 }
            };
            return Ok(products);
        }
    }
}
