using AutoMapper;
using System.Threading.Tasks;
using XplicityApp.Dtos.Surveys.Questions.Answers;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class AnswersService : IAnswersService
    {
        private readonly IMapper _mapper;
        private readonly IAnswerRepository _answerRepository;

        public AnswersService(IMapper mapper, IAnswerRepository answerRepository)
        {
            _mapper = mapper;
            _answerRepository = answerRepository;
        }

        public async Task<int> Create(AnswerDto[] answersDto)
        {
            var answers = _mapper.Map<Answer[]>(answersDto);

            foreach(var answer in answers)
            {
                await _answerRepository.Create(answer);
            }

            return 1;
        }
    }
}
