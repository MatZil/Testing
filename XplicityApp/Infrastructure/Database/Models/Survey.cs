using System.ComponentModel.DataAnnotations;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class Survey : BaseEntity
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public SurveyTypeEnum Type { get; set; }
        [Required]
        public string Guid { get; set; }
    }
}

