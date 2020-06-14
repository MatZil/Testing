namespace XplicityApp.Dtos.Surveys.Questions.Choices
{
    public class GetChoiceDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string ChoiceText { get; set; }
    }
}
