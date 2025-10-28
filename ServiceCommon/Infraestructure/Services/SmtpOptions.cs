namespace ServiceCommon.Infrastructure.Services
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public bool UseSsl { get; set; } = true;
        public string Username { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
