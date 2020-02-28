using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class NotificationSettings : BaseEntity
    {
        [Required]
        [DefaultValue(true)]
        public bool BroadcastOwnBirthday { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool ReceiveBirthdayNotifications { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
