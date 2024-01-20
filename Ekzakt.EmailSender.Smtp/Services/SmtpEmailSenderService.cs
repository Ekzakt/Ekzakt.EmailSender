using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Contracts;
using Microsoft.Extensions.Options;
using FluentValidation;
using Ekzakt.EmailSender.Smtp.Extensions;

namespace Ekzakt.EmailSender.Smtp.Services;

public class SmtpEmailSenderService : IEmailSenderService
{
    private readonly ILogger<SmtpEmailSenderService> _logger;
    private readonly SmtpEmailSenderOptions _options;
    private readonly IValidator<SendEmailRequest> _sendEmailRequstValidator;

    private SendEmailRequest _sendEmailRequest = new();

    public SmtpEmailSenderService(
        ILogger<SmtpEmailSenderService> logger, 
        IOptions<SmtpEmailSenderOptions> options, 
        IValidator<SmtpEmailSenderOptions> optionsValidator,
        IValidator<SendEmailRequest> sendEmailRequstValidator)
    {
        _logger = logger;
        _options = options.Value;
        _sendEmailRequstValidator = sendEmailRequstValidator;

        optionsValidator.ValidateAndThrow(_options);
    }


    public async Task<SendEmailResponse> SendAsync(SendEmailRequest sendEmailRequest, CancellationToken cancellationToken = default)
    {
        _sendEmailRequstValidator.ValidateAndThrow(sendEmailRequest);

        _sendEmailRequest = sendEmailRequest;

        return await SendAsync();
    }


    #region Helpers

    private async Task<SendEmailResponse> SendAsync(CancellationToken cancellationToken = default)
    {
        MimeMessage mimeMessage = _sendEmailRequest.ToMimeMessage(_options.FromDisplayName, _options.FromAddress);

        using var smtp = new SmtpClient();

        try
        {
            _logger.LogInformation("Sending email with subject \"{0}\" to \"{1}\".", _sendEmailRequest.Subject, _sendEmailRequest.Tos?.FirstOrDefault()?.Address);


            _logger.LogDebug("Connecting to SMTP-server {0.Host} on port {1}.", _options.Host, _options.Port);
            await smtp.ConnectAsync(_options.Host, _options.Port, cancellationToken: cancellationToken);
            _logger.LogDebug("Connected successfully.");


            _logger.LogDebug("Authenticating SMPT-server.");
            await smtp.AuthenticateAsync(_options.UserName, _options.Password);
            _logger.LogDebug("Authenticated successfully.");


            _logger.LogDebug("Sending email.");
            var result = await smtp.SendAsync(mimeMessage);
            _logger.LogInformation("Email successfully sent with response {0}.", result);


            return new SendEmailResponse(result);

        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong whiel sending and email. Exception: {0}", ex);

            return new SendEmailResponse(ex.Message);
        }
        finally
        {
            _logger.LogDebug("Disonnecting from SMTP-server.");
            await smtp.DisconnectAsync(true);
        }
    }

    #endregion Helpers
}
