namespace Services.Options
{
    public class SmtpOptions
    {
        public string EmailRecipient { get; set; }
        public string EmailFrom { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
