using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XplicityApp.Dtos.Surveys.Questions.Answers;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswersService _answersService;

        public AnswersController(IAnswersService answersService)
        {
            _answersService = answersService;
        }

        [HttpPost]
        [Produces(typeof(int))]
        public async Task<IActionResult> Post(AnswerDto[] answersDto)
        {
            await _answersService.Create(answersDto);

            return Ok();
        }
    }
}