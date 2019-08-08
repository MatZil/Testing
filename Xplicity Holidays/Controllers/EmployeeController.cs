using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos;
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

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(AuthenticateDTO request)
        {
            var employee = _service.Authenticate(request.Email, request.Password);

            if (employee == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                employee.Id,
                Username = employee.Name,
                employee.Token
            });
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
        public async Task<IActionResult> Put(int id, [FromBody] NewEmployeeDto newEmployeeDto)
        {
            await _service.Update(id, newEmployeeDto);

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

