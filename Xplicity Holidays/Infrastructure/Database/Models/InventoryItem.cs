using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class InventoryItem : BaseEntity
    {
        [Required]
        public string Name { get;set; }

        [Required]
        public string SerialNumber { get; set; }
        
        [Required]
        public DateTime PurchaseDate { get;set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public string Comment { get; set; }

        public int InventoryCategoryId { get; set; }

        public InventoryCategory Category { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
