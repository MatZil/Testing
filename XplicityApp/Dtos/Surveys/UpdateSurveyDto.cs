using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Dtos.Surveys
{
    public class UpdateSurveyDto
    {
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public SurveyTypeEnum Type { get; set; }
    }
}