using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : Controller
    {
        private readonly ISurveysService _surveysService;

        public SurveysController(ISurveysService surveysService)
        {
            _surveysService = surveysService;
        }

        // GET: api/Surveys
        [HttpGet]
        [Produces(typeof(GetSurveyDto[]))]
        public async Task<IActionResult> Get()
        {
            var surveys = await _surveysService.GetAll();

            return Ok(surveys);
        }

        // GET: api/Surveys/5
        [HttpGet("{id}")]
        [Produces(typeof(GetSurveyDto))]
        public async Task<IActionResult> Get(int id)
        {
            var survey = await _surveysService.GetById(id);

            if (survey == null)
                return NotFound();

            return Ok(survey);
        }

        // PUT: api/Surveys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateSurveyDto newSurvey)
        {
            await _surveysService.Update(id, newSurvey);

            return NoContent();
        }

        // POST: api/Surveys
        [HttpPost]
        [Produces(typeof(NewSurveyDto))]
        public async Task<IActionResult> Post(NewSurveyDto newSurvey)
        {
            var createdSurvey = await _surveysService.Create(newSurvey);

            return Ok(createdSurvey);
        }

        // DELETE: api/Surveys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _surveysService.Delete(id);

            return NoContent();
        }
    }
}