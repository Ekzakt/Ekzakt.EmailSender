namespace Ekzakt.EmailSender.Core.Models.Responses;

public class SendEmailResponse
{
    public Guid? Id { get; set; }

    public string? ServerResponse { get; init; }

    public bool IsSuccess => ServerResponse is null ? false : ServerResponse.StartsWith("2");


    [Obsolete("Use SendEmailResponse overload instead. This constructor will be removed in a future version.")]
    public SendEmailResponse(string? serverResponse)
    {
        ServerResponse = serverResponse;
    }


    public SendEmailResponse(Guid? id, string? serverResponse)
    {
        Id = id;
        ServerResponse = serverResponse;
    }
}