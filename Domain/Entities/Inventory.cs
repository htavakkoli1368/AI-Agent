using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inventory
    {
        public int Id { get; private set; }

        public int ProductId { get; private set; }

        public int QuantityOnHand { get; private set; }

        public int QuantityReserved { get; private set; }

        public DateTime UpdatedAtUtc { get; private set; }

        // Navigation property
        public Product Product { get; private set; } = null!;

        public int AvailableQuantity =>
            QuantityOnHand - QuantityReserved;

        // Required by EF Core
        private Inventory()
        {
        }

        public Inventory(int productId, int initialQuantity = 0)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId));

            if (initialQuantity < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(initialQuantity));

            ProductId = productId;
            QuantityOnHand = initialQuantity;
            QuantityReserved = 0;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be positive.");

            QuantityOnHand = checked(QuantityOnHand + quantity);
            Touch();
        }

        public void ReserveStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be positive.");

            if (AvailableQuantity < quantity)
                throw new InvalidOperationException(
                    "Insufficient available stock.");

            QuantityReserved = checked(QuantityReserved + quantity);
            Touch();
        }

        public void ReleaseReservedStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be positive.");

            if (QuantityReserved < quantity)
                throw new InvalidOperationException(
                    "Cannot release more stock than is reserved.");

            QuantityReserved -= quantity;
            Touch();
        }

        public void CommitReservedStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be positive.");

            if (QuantityReserved < quantity)
                throw new InvalidOperationException(
                    "Insufficient reserved stock.");

            if (QuantityOnHand < quantity)
                throw new InvalidOperationException(
                    "Insufficient stock on hand.");

            QuantityReserved -= quantity;
            QuantityOnHand -= quantity;
            Touch();
        }

        private void Touch()
        {
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
