﻿using Ekzakt.EmailSender.Core.EventArguments;
using Ekzakt.EmailSender.Core.Models.Requests;
using Ekzakt.EmailSender.Core.Models.Responses;

namespace Ekzakt.EmailSender.Core.Contracts;

public interface IEkzaktEmailSenderService
{
    delegate Task AsyncEventHandler<TEventArgs>(TEventArgs e);

    event AsyncEventHandler<BeforeSendEmailEventArgs> BeforeEmailSentAsync;

    event AsyncEventHandler<AfterSendEmailEventArgs> AfterEmailSentAsync;

    Task<SendEmailResponse> SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}
