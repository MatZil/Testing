using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IPdfService
    {
        void CreateRequestPdf(Holiday holiday, Employee employee);
        void CreateOrderPdf(Holiday holiday, Employee employee);
    }
}
