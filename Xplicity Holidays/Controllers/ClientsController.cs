using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos.Clients;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        // GET: api/Clients
        [HttpGet]
        [Produces(typeof(GetClientDto[]))]
        public async Task<IActionResult> Get()
        {
            var clients = await _clientsService.GetAll();
            return Ok(clients);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        [Produces(typeof(GetClientDto))]
        public async Task<IActionResult> Get(int id)
        {
            var client = await _clientsService.GetById(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewClientDto newClient)
        {
            await _clientsService.Update(id, newClient);

            return NoContent();
        }

        // POST: api/Clients
        [HttpPost]
        [Produces(typeof(NewClientDto))]
        public async Task<IActionResult> Post(NewClientDto newClient)
        {
            var createdClient = await _clientsService.Create(newClient);

            return Ok(createdClient);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientsService.Delete(id);

            return NoContent();
        }

    }
}
