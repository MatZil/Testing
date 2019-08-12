using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PolicyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetPolicyFile()
        {
            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Pdfs\";
            var fileName = $"Holidays Policy.pdf";
            var stream = new FileStream(path + fileName, FileMode.Open);
            return File(stream, "application/pdf", fileName);
        }
    }
}