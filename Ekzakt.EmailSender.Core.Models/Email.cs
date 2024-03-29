using System.Text.Json.Serialization;

namespace Ekzakt.EmailSender.Core.Models;

public class Email
{
    public Guid Id { get; private set; } = Guid.Empty;

    public EmailAddress From { get; set; } = new();

    public EmailAddress Sender { get; set; } = new();

    public List<EmailAddress> Tos { get; set; } = new();

    public List<EmailAddress>? Ccs { get; set; } = new();

    public List<EmailAddress>? Bccs { get; set; } = new();

    public string Subject { get; set; } = string.Empty;

    public EmailBody Body { get; set; } = new();


    public void SetEmailId(Guid id)
    {
        if (Id.Equals(Guid.Empty))
        {
            Id = id;
        }
    }


    [JsonIgnore]
    public bool HasFromAddress => !string.IsNullOrEmpty(From.Address);


    [JsonIgnore]
    public bool HasSenderAddress => !string.IsNullOrEmpty(Sender.Address);

    
}
