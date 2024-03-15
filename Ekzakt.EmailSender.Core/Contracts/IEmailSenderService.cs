﻿using Ekzakt.EmailSender.Core.EventArguments;
using Ekzakt.EmailSender.Core.Models;

namespace Ekzakt.EmailSender.Core.Contracts;

[Obsolete("Use IEkzaktEmailService instead.  This class will be deleted in a future version.")]
public interface IEmailSenderService
{
    delegate Task AsyncEventHandler<TEventArgs>(TEventArgs e);

    event AsyncEventHandler<BeforeSendEmailEventArgs> BeforeEmailSentAsync;

    event AsyncEventHandler<AfterSendEmailEventArgs> AfterEmailSentAsync;

    Task<SendEmailResponse> SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}
