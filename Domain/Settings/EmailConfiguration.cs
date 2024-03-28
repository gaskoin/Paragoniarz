using Newtonsoft.Json;

namespace Paragoniarz.Domain.Settings
{
    public class EmailConfiguration
    {
        public string Subject { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string User { get; set; }
        [JsonConverter(typeof(PasswordConverter))]
        public string Password { get; set; }
        public string ImapHost { get; set; }
        public int ImapPort { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool UseRecipientName { get; set; }
        public bool SendAllEmailsToSelf { get; set; }
    }
}
