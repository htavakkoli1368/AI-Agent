using Application.DTOs;
using Application.DTOs.Products;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> CreateAsync(
            CreateProductRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var sku = request.Sku.Trim();

            var existingProduct = await _productRepository.GetBySkuAsync(
                sku, cancellationToken);

            if (existingProduct is not null)
                throw new InvalidOperationException(
                    $"A product with SKU '{sku}' already exists.");

            var product = new Product(
                request.Name,
                sku,
                request.Price,
                request.Description);

            await _productRepository.AddAsync(product, cancellationToken);
            await _productRepository.SaveChangesAsync(cancellationToken);

            return MapToResponse(product);
        }

        public async Task<ProductResponse> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var product = await _productRepository.GetByIdAsync(
                id, cancellationToken);

            if (product is null)
                throw new KeyNotFoundException(
                    $"Product with ID '{id}' was not found.");

            return MapToResponse(product);
        }

        public async Task<PagedResult<ProductResponse>> GetPagedAsync(
            ProductListRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Page < 1)
                throw new ArgumentOutOfRangeException(nameof(request.Page));

            if (request.PageSize is < 1 or > 100)
                throw new ArgumentOutOfRangeException(nameof(request.PageSize));

            var search = string.IsNullOrWhiteSpace(request.Search)
                ? null
                : request.Search.Trim();

            var products = await _productRepository.GetPagedAsync(
                search,
                request.ActiveOnly,
                request.Page,
                request.PageSize,
                cancellationToken);

            var totalCount = await _productRepository.CountAsync(
                search,
                request.ActiveOnly,
                cancellationToken);

            return new PagedResult<ProductResponse>
            {
                Items = products.Select(MapToResponse).ToList(),
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ProductResponse> UpdateAsync(
            int id,
            UpdateProductRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var product = await GetTrackedProductAsync(id, cancellationToken);

            var sku = request.Sku.Trim();

            var existingProduct = await _productRepository.GetBySkuAsync(
                sku, cancellationToken);

            if (existingProduct is not null && existingProduct.Id != id)
            {
                throw new InvalidOperationException(
                    $"A product with SKU '{sku}' already exists.");
            }

            product.UpdateDetails(
                request.Name,
                sku,
                request.Description);

            await _productRepository.SaveChangesAsync(cancellationToken);

            return MapToResponse(product);
        }

        public async Task<ProductResponse> ChangePriceAsync(
            int id,
            decimal newPrice,
            CancellationToken cancellationToken = default)
        {
            var product = await GetTrackedProductAsync(id, cancellationToken);

            product.ChangePrice(newPrice);

            await _productRepository.SaveChangesAsync(cancellationToken);

            return MapToResponse(product);
        }

        public async Task SetActiveAsync(
            int id,
            bool isActive,
            CancellationToken cancellationToken = default)
        {
            var product = await GetTrackedProductAsync(id, cancellationToken);

            if (isActive)
                product.Activate();
            else
                product.Deactivate();

            await _productRepository.SaveChangesAsync(cancellationToken);
        }

        private async Task<Product> GetTrackedProductAsync(
            int id,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var product = await _productRepository.GetByIdAsync(
                id, cancellationToken);

            if (product is null)
                throw new KeyNotFoundException(
                    $"Product with ID '{id}' was not found.");

            return product;
        }

        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                Description = product.Description,
                IsActive = product.IsActive,
                CreatedAtUtc = product.CreatedAtUtc
            };
        }
    }
}
