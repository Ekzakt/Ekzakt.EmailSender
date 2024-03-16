namespace Ekzakt.EmailSender.Core.Models.Requests;

public class SendEmailRequest
{
    public Email Email { get; set; } = new();
}
