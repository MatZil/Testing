namespace Xplicity_Holidays.Infrastructure.PdfGeneration
{
    public interface IPdfGenerator
    {
        void GeneratePdf(string htmlString, int holidayId);
    }
}
