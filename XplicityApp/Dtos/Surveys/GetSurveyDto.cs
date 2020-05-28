using System;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Dtos.Surveys
{
    public class GetSurveyDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public SurveyTypeEnum Type { get; set; }
        public string Guid { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
