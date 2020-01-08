using System.Collections.Generic;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class InventoryCategory : BaseEntity
    {
        public string Name { get; set; }

        public int? Deprecation { get; set; }

        public List<InventoryItem> Items { get; set; }
    }
}
