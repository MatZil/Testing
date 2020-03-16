using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Pages
{
    [UsedImplicitly]
    public class HolidayConfirmationModel : PageModel
    {
        private readonly HolidayDbContext _context;

        public int ConfirmerId { get; private set; }
        public int HolidayId { get; private set; }

        //public Employee Employee { get; private set; }
        public string RequesterName { get; private set; }
        public DateTime HolidayFrom { get; private set; }
        public DateTime HolidayTo { get; private set; }

        [BindProperty]
        public string RejectionReason { get; set; }

        public bool IsConfirmerAdmin { get; private set; }

        public HolidayConfirmationModel(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int holidayId, int confirmerId)
        {
            //url for this page is {{RootUrl}}/HolidayConfirmation?holidayid=X&confirmerid=Y
            ConfirmerId = confirmerId;
            HolidayId = holidayId;

            var holiday = await _context.Holidays.FindAsync(holidayId);
            HolidayFrom = holiday.FromInclusive;
            HolidayTo = holiday.ToInclusive;

            var employee = await _context.Employees.FindAsync(holiday.EmployeeId);
            RequesterName = $"{employee.Name} {employee.Surname}";

            if (employee.ClientId == null || holiday.Status == HolidayStatus.ClientConfirmed)
            {
                IsConfirmerAdmin = true;
            }
            else
            {
                IsConfirmerAdmin = false;
            }
        }

        public IActionResult OnPostAsync(bool confirm, int holidayId, int confirmerId, bool isConfirmerAdmin)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Console.WriteLine($"{RejectionReason} {confirm} {holidayId} {confirmerId} {isConfirmerAdmin}");
            //consider creating a holiday confirmation object to call the service with
            //_holidayConfirmationService.UpdateHolidayConfirmationStatus(confirm, holidayId, confirmerId, isConfirmerAdmin, RejectionReason);

            return RedirectToPage("/HolidayConfirmationResult");
        }
    }
}