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
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
             await _context.Products.AddAsync(product, cancellationToken);
        }

        public async Task<int> CountAsync(string? search, bool activeOnly, CancellationToken cancellationToken = default)
        {
              var query = _context.Products.AsNoTracking(); 
              if(activeOnly)              
                    query = query.Where(p => p.IsActive);
              if(!string.IsNullOrWhiteSpace(search))
                    query = query.Where(
                        p => p.Name.Contains(search)
                        || p.Sku.Contains(search)
                        ||(p.Description != null && p.Description.Contains(search))
                        );
              return await query.CountAsync(cancellationToken);

        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
             return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _context.Products               
               .FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetPagedAsync(string? search, bool activeOnly, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Products.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(
                        p => p.Name.Contains(search)
                        || p.Sku.Contains(search)
                        || (p.Description != null && p.Description.Contains(search))
                        );
            }
           
            return  await query.OrderBy(p =>p.Name)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
