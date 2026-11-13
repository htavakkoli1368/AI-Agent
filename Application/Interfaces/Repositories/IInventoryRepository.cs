using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        Task<Inventory?> GetByProductIdAsync(int productId,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Inventory>> GetAllAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
