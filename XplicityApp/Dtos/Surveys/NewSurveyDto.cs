using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Dtos.Surveys
{
    public class NewSurveyDto
    {
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public SurveyTypeEnum Type { get; set; }
    }
}