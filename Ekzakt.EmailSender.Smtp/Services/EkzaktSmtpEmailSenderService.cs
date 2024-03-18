using Ekzakt.EmailSender.Core.Contracts;
using Ekzakt.EmailSender.Core.EventArguments;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Models.Requests;
using Ekzakt.EmailSender.Core.Models.Responses;
using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailSender.Smtp.Extensions;
using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading;

namespace Ekzakt.EmailSender.Smtp.Services;

public class EkzaktSmtpEmailSenderService : IEkzaktEmailSenderService
{
    private readonly ILogger<EkzaktSmtpEmailSenderService> _logger;
    private readonly EkzaktSmtpEmailSenderOptions _options;
    private readonly IValidator<EkzaktSmtpEmailSenderOptions> _smtpEmailSenderOptionsValidator;
    private readonly IValidator<SendEmailRequest> _sendEmailRequestValidator;

    public event IEkzaktEmailSenderService.AsyncEventHandler<BeforeSendEmailEventArgs>? BeforeEmailSentAsync;
    public event IEkzaktEmailSenderService.AsyncEventHandler<AfterSendEmailEventArgs>? AfterEmailSentAsync;

    public EkzaktSmtpEmailSenderService(
        ILogger<EkzaktSmtpEmailSenderService> logger, 
        IOptions<EkzaktSmtpEmailSenderOptions> options, 
        IValidator<EkzaktSmtpEmailSenderOptions> smtpEmailSenderOptionsvalidator,
        IValidator<SendEmailRequest> sendEmailRequstValidator)
    {
        _logger = logger;
        _options = options.Value;
        _smtpEmailSenderOptionsValidator = smtpEmailSenderOptionsvalidator;
        _sendEmailRequestValidator = sendEmailRequstValidator;
    }

    public async Task<SendEmailResponse> SendAsync(SendEmailRequest sendEmailRequest, CancellationToken cancellationToken = default)
    {
        sendEmailRequest.Email.SetEmailId(Guid.NewGuid());

        var eventMessage = string.Empty;

        using var smtp = new SmtpClient();

        try
        {
            _logger.LogInformation("Attempting to send an email with id {EmailId} subject \"{EmailSubject}\" to \"{To}\".", sendEmailRequest.Email.Id, sendEmailRequest.Email.Subject, sendEmailRequest.Email.Tos?.FirstOrDefault()?.Address);

            _smtpEmailSenderOptionsValidator.ValidateAndThrow(_options);

            await OnBeforeEmailSentAsync(new BeforeSendEmailEventArgs
            {
                Id = sendEmailRequest.Email.Id,
                Email = sendEmailRequest.Email
            });

            if (!sendEmailRequest.Email.HasSenderAddress)
            {
                sendEmailRequest.Email.Sender = new EmailAddress(_options.SenderAddress, _options.SenderDisplayName);
            }

            _sendEmailRequestValidator.ValidateAndThrow(sendEmailRequest);

            MimeMessage message = sendEmailRequest.ToMimeMessage();



            await SmtpConnectAsync(smtp, cancellationToken);

            await SmtpAuthenciateAsync(smtp, cancellationToken);

            var result = await SmtpSendAsync(smtp, message, sendEmailRequest.Email.Id, cancellationToken);

            eventMessage = result;

            return new SendEmailResponse(sendEmailRequest.Email.Id, result);

        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while sending the email with id {EmailId}. Exception: {Exception}", sendEmailRequest.Email.Id, ex);

            eventMessage = $"Unexpected error. ({ex.GetType().Name })";

            return new SendEmailResponse(sendEmailRequest.Email.Id, ex.Message);
        }
        finally
        {
            _logger.LogDebug("Disonnecting from SMTP-server.");

            await OnAfterEmailSentAsync(new AfterSendEmailEventArgs
            {
                Id = sendEmailRequest.Email.Id,
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


    private async Task SmtpConnectAsync(SmtpClient smtpClient, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Connecting to SMTP-server {0.Host} on port {1}.", _options.Host, _options.Port);
        await smtpClient.ConnectAsync(_options.Host, _options.Port, cancellationToken: cancellationToken);
        _logger.LogDebug("Connected successfully.");
    }


    private async Task SmtpAuthenciateAsync(SmtpClient smtpClient, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Authenticating SMPT-server.");
        await smtpClient.AuthenticateAsync(_options.Username, _options.Password);
        _logger.LogDebug("Authenticated successfully.");
    }


    private async Task<string?> SmtpSendAsync(SmtpClient smtpClient, MimeMessage mimeMessage, Guid? emailId, CancellationToken cancellationToken)
    {
        var id = emailId ?? Guid.NewGuid();

         _logger.LogDebug("Sending email width id {EmailId}.", id);

        var serverResponse = await smtpClient.SendAsync(mimeMessage, cancellationToken);

        _logger.LogDebug("Email width id {EmailId} successfully sent with response {ServerResponse}.", id, serverResponse);

        return serverResponse;
    }


    #endregion Helpers
}
