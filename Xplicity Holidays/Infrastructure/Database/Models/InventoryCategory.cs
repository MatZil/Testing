using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class InventoryCategory : BaseEntity
    {
        public string Name { get; set; }

        public int Normative { get; set; }

        public List<InventoryItem> Items { get; set; }
    }
}
