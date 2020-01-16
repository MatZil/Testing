using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XplicityApp.Dtos.EmailTemplates;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplatesController: ControllerBase
    {
        private readonly IEmailTemplatesService _emailTemplatesService;

        public EmailTemplatesController(IEmailTemplatesService emailTemplatesService)
        {
            _emailTemplatesService = emailTemplatesService;
        }

        // GET: api/EmailTemplates
        [HttpGet]
        [Produces(typeof(GetEmailTemplateDto[]))]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Get()
        {
            var emailTemplates = await _emailTemplatesService.GetAll();

            return Ok(emailTemplates);
        }

        // GET: api/EmailTemplates/5
        [HttpGet("{id}")]
        [Produces(typeof(GetEmailTemplateDto))]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Get(int id)
        {
            var emailTemplateDto = await _emailTemplatesService.GetById(id);

            if (emailTemplateDto == null)
                return NotFound();

            return Ok(emailTemplateDto);
        }

        // PUT: api/EmailTemplates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewEmailTemplateDto newEmailTemplate)
        {
            await _emailTemplatesService.Update(id, newEmailTemplate);

            return NoContent();
        }

        // POST: api/EmailTemplates
        [HttpPost]
        [Produces(typeof(NewEmailTemplateDto))]
        public async Task<IActionResult> Post(NewEmailTemplateDto newEmailTemplate)
        {
            var createdTemplateDto = await _emailTemplatesService.Create(newEmailTemplate);

            return Ok(createdTemplateDto);
        }

        // DELETE: api/EmailTemplates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _emailTemplatesService.Delete(id);

            return NoContent();
        }
    }
}
