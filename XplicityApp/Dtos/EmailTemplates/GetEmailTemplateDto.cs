namespace XplicityApp.Dtos.EmailTemplates
{
    public class GetEmailTemplateDto
    {
        public int Id { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
        public string Instructions { get; set; }
    }
}
