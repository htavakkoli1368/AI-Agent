using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum OrderStatus
    {
        Pending = 1,
        Confirmed = 2,
        Processing = 3,
        Shipped = 4,
        Delivered = 5,
        Cancelled = 6
    }
    public class Order
    {
        public int Id { get; private set; }

        // The authenticated customer's identifier.
        // Its type should match the identity system used by the API.
        public string CustomerId { get; private set; } = string.Empty;

        public OrderStatus Status { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime? UpdatedAtUtc { get; private set; }

        private readonly List<OrderItem> _items = [];

        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public decimal TotalAmount =>
            _items.Sum(item => item.LineTotal);

        // Required by EF Core
        private Order()
        {
        }

        public Order(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException(
                    "Customer ID is required.", nameof(customerId));

            CustomerId = customerId.Trim();
            Status = OrderStatus.Pending;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public void AddItem(Product product, int quantity)
        {
            ArgumentNullException.ThrowIfNull(product);

            EnsureEditable();

            if (!product.IsActive)
                throw new InvalidOperationException(
                    "Cannot add an inactive product to an order.");

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity), "Quantity must be positive.");

            var existingItem = _items.FirstOrDefault(
                item => item.ProductId == product.Id);

            if (existingItem is not null)
            {
                existingItem.ChangeQuantity(
                    checked(existingItem.Quantity + quantity));
            }
            else
            {
                _items.Add(new OrderItem(
                    product.Id,
                    quantity,
                    product.Price));
            }

            Touch();
        }

        public void RemoveItem(int productId)
        {
            EnsureEditable();

            var item = _items.FirstOrDefault(
                item => item.ProductId == productId);

            if (item is null)
                throw new InvalidOperationException(
                    "The product is not in this order.");

            _items.Remove(item);
            Touch();
        }

        public void Confirm()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending orders can be confirmed.");

            if (_items.Count == 0)
                throw new InvalidOperationException(
                    "Cannot confirm an empty order.");

            Status = OrderStatus.Confirmed;
            Touch();
        }

        public void StartProcessing()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException(
                    "Only confirmed orders can be processed.");

            Status = OrderStatus.Processing;
            Touch();
        }

        public void Ship()
        {
            if (Status != OrderStatus.Processing)
                throw new InvalidOperationException(
                    "Only processing orders can be shipped.");

            Status = OrderStatus.Shipped;
            Touch();
        }

        public void Deliver()
        {
            if (Status != OrderStatus.Shipped)
                throw new InvalidOperationException(
                    "Only shipped orders can be delivered.");

            Status = OrderStatus.Delivered;
            Touch();
        }

        public void Cancel()
        {
            if (Status is OrderStatus.Shipped
                or OrderStatus.Delivered
                or OrderStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "This order cannot be cancelled.");
            }

            Status = OrderStatus.Cancelled;
            Touch();
        }

        private void EnsureEditable()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending orders can be edited.");
        }

        private void Touch()
        {
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
