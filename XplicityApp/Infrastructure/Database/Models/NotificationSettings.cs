using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class NotificationSettings : BaseEntity
    {
        [Required]
        public bool BroadcastOwnBirthday { get; set; }
        [Required]
        public bool ReceiveBirthdayNotifications { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
