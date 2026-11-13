using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Inventory
{
    public class AdjustInventoryRequest
    {
        [Range(1, 100000)]
        public int Quantity { get; set; }
    }
}
