using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Emailer;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class EmailService: IEmailService
    {
        private readonly IEmailer _emailer;

        public EmailService(IEmailer emailer)
        {
            _emailer = emailer;
        }
        public void ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday)
        {
            _emailer.SendMail(client.OwnerEmail, "A holiday request from your employee",
                $"Hello, {client.OwnerName}\nOne of your employees, {employee.Name} {employee.Surname}, is intending to go " +
                $"on {((holiday.Type.ToString() == "Paternal") ? "Paternal/Maternal" : holiday.Type.ToString())} " +
                $"holidays from {holiday.FromInclusive.ToShortDateString()} (inclusive) to " +
                $"{holiday.ToExclusive.ToShortDateString()} (exclusive).\n" +
                $"Click this link to confirm the holiday: https://localhost:44374/api/holidayclient?holidayid={holiday.Id} \n" +
                $"Click this link to decline the holiday: https://localhost:44374/api/holidaydecline?holidayid={holiday.Id}");
        }

        public void ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus)
        {
            _emailer.SendMail(admin.Email, "A holiday request from an employee",
                $"Hello, {admin.Name},\nAn employee {employee.Name} {employee.Surname} is intending to go on " +
                $"{((holiday.Type.ToString() == "Paternal") ? "Paternal/Maternal" : holiday.Type.ToString())} " +
                $"holidays from {holiday.FromInclusive.ToShortDateString()} (inclusive) to {holiday.ToExclusive.ToShortDateString()} " +
                $"(exclusive). " + clientStatus +
                $"\nClick this link to confirm the holiday: https://localhost:44374/api/holidayconfirm?holidayid={holiday.Id} \n" +
                $"Click this link to decline the holiday: https://localhost:44374/api/holidaydecline?holidayid={holiday.Id}");
        }
    }
}
