using CVEmailLib;
namespace CVEmailAPI
{
    public class CVEmail
    {
        public string? smtpServer { get; set; }
        public int smtpPort { get; set; }
        public string? smtpUsername { get; set; }
        public string? smtpPassword { get; set; }
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }

    }
}
