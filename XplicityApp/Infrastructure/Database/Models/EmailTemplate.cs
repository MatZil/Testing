using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class EmailTemplate: BaseEntity
    {
        [Required]
        public string Purpose { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Template { get; set; }

        [Required]
        public string Instructions { get; set; }
    }
}
