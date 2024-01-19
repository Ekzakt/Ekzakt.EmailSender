namespace Ekzakt.EmailSender.Core.Models;

public class SendEmailResponse
{
    public string? ServerResponse { get; set; }

    public bool IsSuccess => ServerResponse is null ? false : ServerResponse.Contains(" queued as");
}