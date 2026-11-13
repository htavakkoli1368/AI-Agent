using Application.DTOs.Inventory;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _repository = inventoryRepository;
        }
        public async Task<InventoryResponse?> GetByProductIdAsync(int productId,CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByProductIdAsync(productId, cancellationToken);
            return inventory is null ? null : MapToResponse(inventory);
        }

        public async Task<IReadOnlyList<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var inventories = await _repository.GetAllAsync(cancellationToken);

            return inventories.Select(MapToResponse).ToList();
        }

        public async Task AdjustStockAsync(int productId,AdjustInventoryRequest request,CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByProductIdAsync(productId, cancellationToken);
            if (inventory is null)
                throw new KeyNotFoundException("Inventory was not found.");
            inventory.AddStock(request.Quantity);
            await _repository.SaveChangesAsync(cancellationToken);
        }

        private static InventoryResponse MapToResponse(Domain.Entities.Inventory inventory)
        {
            return new InventoryResponse
            {
                ProductId = inventory.ProductId,
                ProductName = inventory.Product.Name,
                Sku = inventory.Product.Sku,
                QuantityOnHand = inventory.QuantityOnHand,
                QuantityReserved = inventory.QuantityReserved,
                AvailableQuantity = inventory.AvailableQuantity
            };
        }


    }
}
