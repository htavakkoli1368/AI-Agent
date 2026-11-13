using Application.DTOs.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IInventoryService
    {
        Task<InventoryResponse?> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<InventoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AdjustStockAsync(int productId, AdjustInventoryRequest request, CancellationToken cancellationToken = default);
    }
}
