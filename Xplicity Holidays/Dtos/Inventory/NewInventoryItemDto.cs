using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Dtos.Inventory
{
    public class NewInventoryItemDto
    {
        public string Name { get; set; }

        public string SerialNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string Comment { get; set; }

        public int InventoryCategoryId { get; set; }

        public decimal Price { get; set; }

        public int? EmployeeId { get; set; }
    }
}
