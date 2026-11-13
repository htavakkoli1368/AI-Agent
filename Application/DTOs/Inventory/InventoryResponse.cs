using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Inventory
{
    public class InventoryResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;

        public int QuantityOnHand { get; set; }
        public int QuantityReserved { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
