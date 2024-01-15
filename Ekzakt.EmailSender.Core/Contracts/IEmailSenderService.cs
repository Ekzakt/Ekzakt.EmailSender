using Ekzakt.EmailSender.Core.Models;

namespace Ekzakt.EmailSender.Core.Contracts;

public interface IEmailSenderService
{
    Task<SendEmailResponse> SendAsync(SendEmailRequest request);
}
