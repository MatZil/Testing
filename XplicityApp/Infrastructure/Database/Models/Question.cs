using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class Question : BaseEntity
    {
        [Required]
        public int SurveyId { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public QuestionTypeEnum Type { get; set; }
        [Required]
        public ICollection<Choice> Choices { get; set; }
    }
}