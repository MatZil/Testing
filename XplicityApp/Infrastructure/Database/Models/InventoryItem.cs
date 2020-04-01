using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class InventoryItem : BaseEntity
    {
        [Required]
        public string Name { get;set; }

        [Required]
        public string SerialNumber { get; set; }
        
        [Required]
        public DateTime PurchaseDate { get;set; }

        public DateTime? ExpiryDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentPrice { get; set; }

        public string Comment { get; set; }

        public int InventoryCategoryId { get; set; }

        public InventoryCategory Category { get; set; }

        public int? EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public ICollection<InventoryItemTag> InventoryItemsTags { get; set; }

        public bool Archived { get; set; }
    }
}
