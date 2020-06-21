using System.Threading.Tasks;
using XplicityApp.Dtos.Surveys.Questions.Answers;

namespace XplicityApp.Services.Interfaces
{
    public interface IAnswersService
    {
        Task<int> Create(AnswerDto[] answersDto);
    }
}
