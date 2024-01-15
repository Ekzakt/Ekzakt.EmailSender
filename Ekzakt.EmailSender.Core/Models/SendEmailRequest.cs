namespace Ekzakt.EmailSender.Core.Models;

public class SendEmailRequest
{
    public EmailAddress? From { get; set; }
    public List<EmailAddress> Tos { get; set; } = new();
    public List<EmailAddress>? Ccs { get; set; } = new();
    public List<EmailAddress>? Bccs { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string? TextBody { get; set; } = string.Empty;
}
