using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class Tag : BaseEntity
    {
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string Title { get; set; }

        public ICollection<InventoryItemTag> InventoryItemsTags { get; set; }
    }
}
