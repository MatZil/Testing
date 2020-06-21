using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Dtos.Surveys.Answers
{
    public class AnswersDto
    {
        public ICollection<Answer> Answers { get; set; }
    }
}
