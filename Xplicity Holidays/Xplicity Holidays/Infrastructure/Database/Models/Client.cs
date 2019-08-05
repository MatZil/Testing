using System.ComponentModel.DataAnnotations;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class Client: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public Team Team { get; set; }
    }
}
