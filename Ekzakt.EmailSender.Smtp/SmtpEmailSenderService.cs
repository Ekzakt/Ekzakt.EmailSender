using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Contracts;
using Microsoft.Extensions.Options;

namespace Ekzakt.EmailSender.Smtp;

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

    private async Task<SendEmailResponse> SendAsync()
    {
        MimeMessage mimeMessage = BuildMimeMessage();

        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(_options.Host, _options.Port);
            await smtp.AuthenticateAsync(_options.UserName, _options.Password);

            var result = await smtp.SendAsync(mimeMessage);

            return new SendEmailResponse { ServerResponse = result };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);

            return new SendEmailResponse { ServerResponse = ex.Message };
        }
        finally
        {
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
            addressesList.Add(new MailboxAddress(emailAddress.Name, emailAddress.Address));
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
