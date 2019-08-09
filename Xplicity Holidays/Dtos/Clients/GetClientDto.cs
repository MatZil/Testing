namespace Xplicity_Holidays.Dtos.Clients
{
    public class GetClientDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurname { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerPhone { get; set; }
    }
}
