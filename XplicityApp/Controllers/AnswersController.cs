using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return NotFound(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnswerDto[] answersDto)
        {
            await _answersService.Create(answersDto);

            return Ok();
        }
    }
}
