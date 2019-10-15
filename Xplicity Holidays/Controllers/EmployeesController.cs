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
    [Authorize(Roles="admin")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        // GET: api/Employees
        [HttpGet]
        [Produces(typeof(GetEmployeeDto[]))]
        public async Task<IActionResult> Get()
        {
            var clients = await _employeesService.GetAll();

            return Ok(clients);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        [Produces(typeof(GetEmployeeDto))]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employeesService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            await _employeesService.Update(id, updateEmployeeDto);

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        [Produces(typeof(NewEmployeeDto))]
        public async Task<IActionResult> Post(NewEmployeeDto newEmployeeDto)
        {
            var createdEmployee = await _employeesService.Create(newEmployeeDto);

            return Ok(createdEmployee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeesService.Delete(id);

            return NoContent();
        }
    }
}

