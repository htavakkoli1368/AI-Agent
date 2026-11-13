using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Inventory?> GetByProductIdAsync(int productId,CancellationToken cancellationToken = default)
        {
            return await _context.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.ProductId == productId,cancellationToken);
        }

        public async Task<IReadOnlyList<Inventory>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Inventories
                .AsNoTracking()
                .Include(i => i.Product)
                .OrderBy(i => i.Product.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }


    }
}
