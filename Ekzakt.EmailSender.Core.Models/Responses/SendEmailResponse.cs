namespace Ekzakt.EmailSender.Core.Models.Responses;

public class SendEmailResponse
{
    public SendEmailResponse(string serverResponse)
    {
        ServerResponse = serverResponse;    
    }

    public string? ServerResponse { get; init; }

    public bool IsSuccess => ServerResponse is null ? false : ServerResponse.StartsWith("2");
}