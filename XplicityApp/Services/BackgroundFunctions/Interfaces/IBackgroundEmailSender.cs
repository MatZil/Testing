using System.Threading.Tasks;
using System;

namespace XplicityApp.Services.BackgroundFunctions.Interfaces
{
    public interface IBackgroundEmailSender
    {
        Task SendHolidayReports();
        Task BroadcastCoworkersAbsences();
        Task BroadcastCoworkersBirthdays();
    }
}
