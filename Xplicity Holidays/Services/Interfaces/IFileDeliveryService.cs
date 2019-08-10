using System.Net.Http;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IFileDeliveryService
    {
        HttpResponseMessage DeliverFile(string pathToFolder, string fileName, string fileType, string header);
    }
}
