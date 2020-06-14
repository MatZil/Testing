using System.Collections.Generic;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Dtos.Surveys.Questions.Choices;

namespace XplicityApp.Dtos.Surveys.Questions
{
    public class GetQuestionDto
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string QuestionText { get; set; }
        public ICollection<NewChoiceDto> Choices { get; set; }
    }
}