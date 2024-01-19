using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Contracts;
using Microsoft.Extensions.Options;

namespace Ekzakt.EmailSender.Smtp.Services;

public class SmtpEmailSenderService(
    ILogger<SmtpEmailSenderService> logger,
    IOptions<SmtpEmailSenderOptions> options) : IEmailSenderService
{
    private readonly ILogger<SmtpEmailSenderService> _logger = logger;
    private SmtpEmailSenderOptions _options = options.Value;
    private SendEmailRequest _sendEmailRequest = new();


    public async Task<SendEmailResponse> SendAsync(SendEmailRequest sendRequest)
    {
        _sendEmailRequest = sendRequest;

        return await SendAsync();
    }



    #region Helpers

    private async Task<SendEmailResponse> SendAsync(CancellationToken cancellationToken = default)
    {
        MimeMessage mimeMessage = BuildMimeMessage();

        using var smtp = new SmtpClient();

        try
        {
            _logger.LogInformation("Sending email with subject \"{0}\" to \"{1}\".",
                _sendEmailRequest.Subject,
                _sendEmailRequest.Tos?.FirstOrDefault()?.Address);
            _logger.LogDebug("Connecting to SMTP-server {0.Host} on port {1}.", _options.Host, _options.Port);

            await smtp.ConnectAsync(
                host: _options.Host, 
                port: _options.Port, 
                cancellationToken: cancellationToken);
            _logger.LogDebug("Connected successfully.");

            _logger.LogDebug("Authenticating SMPT-server.");
            await smtp.AuthenticateAsync(
                _options.UserName, 
                _options.Password);
            _logger.LogDebug("Authenticated successfully.");

            _logger.LogDebug("Sending email.");
            var result = await smtp.SendAsync(mimeMessage);
            _logger.LogDebug("Email successfully sent with response {0}.", result);

            return new SendEmailResponse { ServerResponse = result };
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong whiel sending and email. Exception: {0}", ex);

            return new SendEmailResponse { ServerResponse = ex.Message };
        }
        finally
        {
            _logger.LogDebug("Disonnecting from SMTP-server.");
            await smtp.DisconnectAsync(true);
        }
    }


    private MimeMessage BuildMimeMessage()
    {
        MimeMessage mimeMessage = new();

        mimeMessage.Sender = new MailboxAddress(
                _sendEmailRequest?.From?.Name ?? _options.FromDisplayName,
                _sendEmailRequest?.From?.Address ?? _options.FromAddress);

        mimeMessage.Subject = GetEmailSubject(_sendEmailRequest?.Subject);

        mimeMessage.To.AddRange(ConvertFromEmailAddressList(_sendEmailRequest?.Tos));
        mimeMessage.Cc.AddRange(ConvertFromEmailAddressList(_sendEmailRequest?.Ccs));
        mimeMessage.Bcc.AddRange(ConvertFromEmailAddressList(_sendEmailRequest?.Bccs));

        mimeMessage.Body = BuildBody();

        return mimeMessage;
    }


    private MimeEntity BuildBody()
    {
        if (string.IsNullOrEmpty(_sendEmailRequest.HtmlBody))
        {
            throw new ArgumentNullException(nameof(BodyBuilder.HtmlBody));
        }

        BodyBuilder bodyBuilder = new();

        bodyBuilder.HtmlBody = _sendEmailRequest.HtmlBody;
        bodyBuilder.TextBody = _sendEmailRequest.TextBody ?? string.Empty;

        return bodyBuilder.ToMessageBody();
    }


    private InternetAddressList ConvertFromEmailAddressList(List<EmailAddress>? emailAddresses)
    {
        InternetAddressList addressesList = new();

        foreach (var emailAddress in emailAddresses ?? new List<EmailAddress>())
        {
            addressesList.Add(new MailboxAddress(emailAddress.Name ?? string.Empty, emailAddress.Address));
        }

        return addressesList;
    }


    private string? GetEmailSubject(string? subject)
    {
        bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        if (isDevelopment)
        {
            return $"*** DEV {subject} ***";
        }
        else
        {
            return subject;
        }
    }


    #endregion Helpers
}
