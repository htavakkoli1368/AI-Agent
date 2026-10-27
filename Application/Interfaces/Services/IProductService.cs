using Application.DTOs;
using Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<ProductResponse> CreateAsync(
    CreateProductRequest request,
    CancellationToken cancellationToken = default);

        Task<ProductResponse> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<PagedResult<ProductResponse>> GetPagedAsync(
            ProductListRequest request,
            CancellationToken cancellationToken = default);

        Task<ProductResponse> UpdateAsync(
            int id,
            UpdateProductRequest request,
            CancellationToken cancellationToken = default);

        Task<ProductResponse> ChangePriceAsync(
            int id,
            decimal newPrice,
            CancellationToken cancellationToken = default);

        Task SetActiveAsync(
            int id,
            bool isActive,
            CancellationToken cancellationToken = default);
    }
}
