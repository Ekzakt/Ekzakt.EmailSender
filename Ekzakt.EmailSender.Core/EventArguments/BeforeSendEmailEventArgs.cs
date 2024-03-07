using Ekzakt.EmailSender.Core.Models;

namespace Ekzakt.EmailSender.Core.EventArguments;

public class BeforeSendEmailEventArgs : EventArgs
{
    public Guid Id { get; init; }

    public SendEmailRequest SendEmailRequest { get; init; } = new();
}
