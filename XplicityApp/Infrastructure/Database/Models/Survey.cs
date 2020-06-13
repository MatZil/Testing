using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class Survey : BaseEntity
    {
        [Required]
        public string Guid { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        public bool AnonymousAnswers { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
