using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; private set; }

        public int OrderId { get; private set; }

        public int ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        // Navigation properties
        public Order Order { get; private set; } = null!;

        public Product Product { get; private set; } = null!;

        public decimal LineTotal => UnitPrice * Quantity;

        // Required by EF Core
        private OrderItem()
        {
        }

        internal OrderItem(
            int productId,
            int quantity,
            decimal unitPrice)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId));

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be positive.");

            if (unitPrice < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(unitPrice), "Unit price cannot be negative.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        internal void ChangeQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be positive.");

            Quantity = quantity;
        }
    }
}
