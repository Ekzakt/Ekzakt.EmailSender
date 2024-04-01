namespace Ekzakt.EmailSender.Core.Models.Requests;

public class SendEmailRequest
{
    public string? TenantId { get; set; }

    public string TemplateName { get; set; } = string.Empty;

    public string RecipientType { get;set; } = string.Empty;

    public Email Email { get; set; } = new();
}
