using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class Answer : BaseEntity
    {
        [Required]
        public int QuestionId { get; set; }
        public int? EmployeeId { get; set; }
        [Required]
        public string AnswerText { get; set; }

    }
}
