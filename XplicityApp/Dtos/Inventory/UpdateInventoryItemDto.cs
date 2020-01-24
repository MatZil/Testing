﻿using System;

namespace XplicityApp.Dtos.Inventory
{
    public class UpdateInventoryItemDto
    {
        public string Name { get; set; }

        public string SerialNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Comment { get; set; }

        public int InventoryCategoryId { get; set; }

        public decimal Price { get; set; }

        public int? EmployeeId { get; set; }
    }
}