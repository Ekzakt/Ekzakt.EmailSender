using Ekzakt.EmailSender.Core.EventArguments;
using Ekzakt.EmailSender.Core.Models;

namespace Ekzakt.EmailSender.Core.Contracts;

public interface IEmailSenderService
{
    delegate Task AsyncEventHandler<TEventArgs>(TEventArgs e);

    event AsyncEventHandler<BeforeSendEmailEventArgs> BeforeEmailSentAsync;

    event AsyncEventHandler<AfterSendEmailEventArgs> AfterEmailSentAsync;

    Task<SendEmailResponse> SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}
