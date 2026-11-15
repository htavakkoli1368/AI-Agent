using Application.DTOs.Inventory;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        // GET: api/inventory
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _inventoryService.GetAllAsync(cancellationToken);

            return Ok(result);
        }
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetByProductId(int productId,CancellationToken cancellationToken)
        {
            var result = await _inventoryService.GetByProductIdAsync(productId, cancellationToken);
            if (result is null)
                return NotFound(new { message = "Inventory not found." });
            return Ok(result);
        }
        // POST: api/inventory/5/adjust
        [HttpPost("{productId:int}/adjust")]
        public async Task<IActionResult> AdjustStock(int productId,[FromBody] AdjustInventoryRequest request,CancellationToken cancellationToken)
        {
            try
            {
                await _inventoryService.AdjustStockAsync(productId, request, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Inventory not found." });
            }
        }
    }
}
