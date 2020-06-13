using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class Choice : BaseEntity
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string ChoiceText { get; set; }
    }
}