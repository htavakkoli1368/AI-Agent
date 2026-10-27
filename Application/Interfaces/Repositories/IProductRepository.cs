using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync( int id,CancellationToken cancellationToken = default);

        Task<Product?> GetBySkuAsync( string sku,CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Product>> GetPagedAsync(string? search,bool activeOnly,int page,int pageSize,CancellationToken cancellationToken = default);

        Task<int> CountAsync(string? search,bool activeOnly,CancellationToken cancellationToken = default);

        Task AddAsync(Product product,CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
