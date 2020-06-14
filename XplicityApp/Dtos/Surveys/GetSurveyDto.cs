using System.Collections.Generic;
using XplicityApp.Dtos.Surveys.Questions;
using System;

namespace XplicityApp.Dtos.Surveys
{
    public class GetSurveyDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public bool AnonymousAnswers { get; set; }
        public string Guid { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<NewQuestionDto> Questions { get; set; }
    }
}