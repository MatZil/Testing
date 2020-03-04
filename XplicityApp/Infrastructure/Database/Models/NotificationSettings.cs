using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class NotificationSettings : BaseEntity
    {
        [Required]
        [DefaultValue(true)]
        public bool BroadcastOwnBirthday { get; set; } = true;
        [Required]
        [DefaultValue(true)]
        public bool ReceiveBirthdayNotifications { get; set; } = true;
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
