using Application.DTOs.Products;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService )
        {
            _productService = productService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromQuery]ProductListRequest request)
        {
            var products = await _productService.GetPagedAsync(request);
            return Ok(products);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);

                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Product was not found."
                });
            }
        }
        // POST: api/products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                var product = await _productService.CreateAsync(request);

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id },product);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        // PUT: api/products/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id,[FromBody] UpdateProductRequest request)
        {
            try
            {
                var product = await _productService.UpdateAsync(id, request);

                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    message = "Product was not found."
                });
            }
        }
    }
}
