using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.Users;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        private readonly IUserService _userService;
        public EmployeesController(IEmployeesService employeesService, IUserService userService)
        {
            _employeesService = employeesService;
            _userService = userService;
        }

        // GET: api/Employees
        [HttpGet]
        [Produces(typeof(GetEmployeeDto[]))]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Get()
        {
            var clients = await _employeesService.GetAll();
            return Ok(clients);
        }

        [HttpGet]
        [Produces(typeof(GetEmployeeDto[]))]
        [Route("GetByStatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus)
        {
            var employees = await _employeesService.GetByEmployeeStatus(employeeStatus);
            return Ok(employees);
        }

        [HttpGet]
        [Route("self")]
        [Authorize]
        public async Task<IActionResult> GetSelf()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userService.GetCurrentUser(userEmail);
            currentUser = _employeesService.AddOvertimeDays(currentUser);

            return Ok(currentUser);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        [Produces(typeof(GetEmployeeDto))]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employeesService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            await _employeesService.Update(id, updateEmployeeDto);

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        [Produces(typeof(NewEmployeeDto))]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Post(NewEmployeeDto newEmployeeDto)
        {
            var createdEmployee = await _employeesService.Create(newEmployeeDto);

            return Ok(createdEmployee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeesService.Delete(id);

            return NoContent();
        }

        [HttpPost("{id}/ChangePassword")]
        public async Task<IActionResult> ChangePassword(int id, UpdatePasswordDto passwordDto)
        {
            await _userService.ChangePassword(id, passwordDto);
            
            return Ok();
        }

        [HttpGet]
        [Produces(typeof(bool))]
        [Route("{email}/exists")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> EmailExists(string email)
        {
            return Ok(await _employeesService.EmailExists(email));
        }
    }
}

