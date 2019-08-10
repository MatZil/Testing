using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class FileDeliveryService: IFileDeliveryService
    {
        public HttpResponseMessage DeliverFile(string pathToFolder, string fileName, string fileType, string header)
        {
            var result = new HttpResponseMessage();
            var fileBytes = File.ReadAllBytes(pathToFolder + @"\" + fileName);
            var fileMemStream = new MemoryStream(fileBytes);
            result.Content = new StreamContent(fileMemStream);
            var headers = result.Content.Headers;
            headers.ContentDisposition = new ContentDispositionHeaderValue(header);
            headers.ContentDisposition.FileName = fileName;
            headers.ContentType = new MediaTypeHeaderValue("application/" + fileType);
            headers.ContentLength = fileMemStream.Length;
            return result;
        }
    }
}
