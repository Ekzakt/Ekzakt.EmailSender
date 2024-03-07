namespace Ekzakt.EmailSender.Core.EventArguments;

public class AfterSendEmailEventArgs : EventArgs
{
    public Guid Id { get; init; }

    public string ResponseMessage { get; init; } = string.Empty;
}
