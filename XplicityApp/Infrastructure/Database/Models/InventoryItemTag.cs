namespace XplicityApp.Infrastructure.Database.Models
{
    public class InventoryItemTag : BaseEntity
    {
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
