using System.Text.Json.Serialization;

namespace Ekzakt.EmailSender.Core.Models;

public class SendEmailRequest
{
    [JsonIgnore]
    [Obsolete("Use Sender instead. From will be removed in future versions.")]
    public EmailAddress From { get; set; } = new();

    public EmailAddress Sender { get; set; } = new();

    public List<EmailAddress> Tos { get; set; } = new();

    public List<EmailAddress>? Ccs { get; set; } = new();

    public List<EmailAddress>? Bccs { get; set; } = new();

    public string Subject { get; set; } = string.Empty;

    public EmailBody Body { get; set; } = new();

    [JsonIgnore]
    public bool HasSender => !string.IsNullOrEmpty(Sender.Address);
}
