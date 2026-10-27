using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public string Sku { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public bool IsActive { get; private set; } = true;

        public DateTime CreatedAtUtc { get; private set; }

        // Navigation property
        public Inventory? Inventory { get; private set; }

        // Required by EF Core
        private Product()
        {
        }

        public Product(
            string name,
            string sku,
            decimal price,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(
                    "Product name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException(
                    "Product SKU is required.", nameof(sku));

            if (price < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(price), "Price cannot be negative.");

            Name = name.Trim();
            Sku = sku.Trim();
            Price = price;
            Description = NormalizeDescription(description);
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void UpdateDetails(
            string name,
            string sku,
            string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(
                    "Product name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException(
                    "Product SKU is required.", nameof(sku));

            Name = name.Trim();
            Sku = sku.Trim();
            Description = NormalizeDescription(description);
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(newPrice), "Price cannot be negative.");

            Price = newPrice;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        private static string? NormalizeDescription(string? description)
        {
            return string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim();
        }
    }
}
