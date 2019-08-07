using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _service;

        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }

        // GET: api/Employees
        [HttpGet]
        [Produces(typeof(GetEmployeeDto[]))]
        public async Task<IActionResult> Get()
        {
            var clients = await _service.GetAll();
            return Ok(clients);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        [Produces(typeof(GetEmployeeDto))]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _service.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewEmployeeDto NewEmployeeDto)
        {
            await _service.Update(id, NewEmployeeDto);

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        [Produces(typeof(NewEmployeeDto))]
        public async Task<IActionResult> Post(NewEmployeeDto newEmployeeDto)
        {
            var createdEmployee = await _service.Create(newEmployeeDto);

            return Ok(createdEmployee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);

            return NoContent();
        }
    }
}
