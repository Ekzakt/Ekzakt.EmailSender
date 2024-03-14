﻿using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Contracts;
using Microsoft.Extensions.Options;
using FluentValidation;
using Ekzakt.EmailSender.Smtp.Extensions;
using Ekzakt.EmailSender.Core.EventArguments;

namespace Ekzakt.EmailSender.Smtp.Services;

public class SmtpEmailSenderService : IEmailSenderService
{
    private readonly ILogger<SmtpEmailSenderService> _logger;
    private readonly SmtpEmailSenderOptions _options;
    private readonly IValidator<SmtpEmailSenderOptions> _smtpEmailSenderOptionsValidator;
    private readonly IValidator<SendEmailRequest> _sendEmailRequestValidator;

    public event IEmailSenderService.AsyncEventHandler<BeforeSendEmailEventArgs>? BeforeEmailSentAsync;
    public event IEmailSenderService.AsyncEventHandler<AfterSendEmailEventArgs>? AfterEmailSentAsync;

    public SmtpEmailSenderService(
        ILogger<SmtpEmailSenderService> logger, 
        IOptions<SmtpEmailSenderOptions> options, 
        IValidator<SmtpEmailSenderOptions> smtpEmailSenderOptionsvalidator,
        IValidator<SendEmailRequest> sendEmailRequstValidator)
    {
        _logger = logger;
        _options = options.Value;
        _smtpEmailSenderOptionsValidator = smtpEmailSenderOptionsvalidator;
        _sendEmailRequestValidator = sendEmailRequstValidator;
    }

    public async Task<SendEmailResponse> SendAsync(SendEmailRequest sendEmailRequest, CancellationToken cancellationToken = default)
    {
        var emailId = Guid.NewGuid();
        var eventMessage = string.Empty;

        using var smtp = new SmtpClient();

        try
        {
            _smtpEmailSenderOptionsValidator.ValidateAndThrow(_options);

            if (!sendEmailRequest.HasSender)
            {
                sendEmailRequest.Sender = new EmailAddress(_options.SenderAddress, _options.SenderDisplayName);
            }

            _sendEmailRequestValidator.ValidateAndThrow(sendEmailRequest);

            MimeMessage mimeMessage = sendEmailRequest.ToMimeMessage();

            await OnBeforeEmailSentAsync(new BeforeSendEmailEventArgs
            { 
                Id = emailId, 
                SendEmailRequest = sendEmailRequest
            });


            _logger.LogInformation("Sending email with subject \"{0}\" to \"{1}\".", sendEmailRequest.Subject, sendEmailRequest.Tos?.FirstOrDefault()?.Address);


            _logger.LogDebug("Connecting to SMTP-server {0.Host} on port {1}.", _options.Host, _options.Port);
            await smtp.ConnectAsync(_options.Host, _options.Port, cancellationToken: cancellationToken);
            _logger.LogDebug("Connected successfully.");


            _logger.LogDebug("Authenticating SMPT-server.");
            await smtp.AuthenticateAsync(_options.Username, _options.Password);
            _logger.LogDebug("Authenticated successfully.");


            _logger.LogDebug("Sending email.");
            var result = await smtp.SendAsync(mimeMessage, cancellationToken);
            _logger.LogInformation("Email successfully sent with response {0}.", result);

            eventMessage = result;

            return new SendEmailResponse(result);

        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while sending and email. Exception: {0}", ex);

            eventMessage = $"Unexpected error. ({ex.GetType().Name })";

            return new SendEmailResponse(ex.Message);
        }
        finally
        {
            _logger.LogDebug("Disonnecting from SMTP-server.");

            await OnAfterEmailSentAsync(new AfterSendEmailEventArgs 
            { 
                Id = emailId, 
                ResponseMessage = eventMessage
            });

            await smtp.DisconnectAsync(true);
        }
    }




    #region Helpers

    private async Task OnBeforeEmailSentAsync(BeforeSendEmailEventArgs e)
    {
        if (BeforeEmailSentAsync is not null)
        {
            _logger.LogDebug("Sending event {0}.", nameof(BeforeEmailSentAsync));
            await BeforeEmailSentAsync(e);
        }
    }


    private async Task OnAfterEmailSentAsync(AfterSendEmailEventArgs e)
    {
        if (AfterEmailSentAsync is not null)
        {
            _logger.LogDebug("Sending event {0}.", nameof(AfterEmailSentAsync));
            await AfterEmailSentAsync(e);
        }
    }

    #endregion Helpers
}
