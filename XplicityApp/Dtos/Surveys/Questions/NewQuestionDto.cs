using System.Collections.Generic;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Dtos.Surveys.Questions.Choices;

namespace XplicityApp.Dtos.Surveys.Questions
{
    public class NewQuestionDto
    {
        public QuestionTypeEnum Type { get; set; }
        public string QuestionText { get; set; }
        public ICollection<ChoiceDto> Choices { get; set; }
    }
}