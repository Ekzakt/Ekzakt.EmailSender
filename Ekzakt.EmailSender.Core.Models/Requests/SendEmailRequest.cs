namespace Ekzakt.EmailSender.Core.Models.Requests;

public class SendEmailRequest
{
    [Obsolete("This method should not be used anymore. It is not solid.")]
    public string? TenantId { get; set; }

    [Obsolete("This method should not be used anymore. It is not solid.")]
    public string TemplateName { get; set; } = string.Empty;

    [Obsolete("This method should not be used anymore. It is not solid.")]
    public string RecipientType { get;set; } = string.Empty;

    public Email Email { get; set; } = new();
}
