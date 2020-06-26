namespace XplicityApp.Dtos.Surveys.Questions.Answers
{
    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int? EmployeeId { get; set; }
        public string AnswerText { get; set; }
    }
}
