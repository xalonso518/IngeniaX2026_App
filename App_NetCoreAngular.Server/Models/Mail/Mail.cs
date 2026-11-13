using System.Net.Mail;

namespace App_NetCoreAngular.Server.Models.Mail
{
    public class Mail
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public List<string>? ToList { get; set; }
        public string? Subject { get; set; }
        public List<string>? SubjectList { get; set; }
        public string? PlainTextContent { get; set; }
        public string? HtmlTextContent { get; set; }
        public Dictionary<string, string>? Substitution { get; set; }
        public Attachment? Attachment { get; set; }
        public List<Attachment>? Attachments { get; set; }
    }
}
