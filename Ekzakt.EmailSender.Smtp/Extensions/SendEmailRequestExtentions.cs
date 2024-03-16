using Ekzakt.EmailSender.Core.Models.Requests;
using MimeKit;

namespace Ekzakt.EmailSender.Smtp.Extensions;

public static class SendEmailRequestExtentions
{
    /// <summary>
    /// This method converts a SendEmailRequest object toa MimeKit.MimeMessage object.
    /// </summary>
    /// <returns>MimeKit.MimeMessage</returns>
    public static MimeMessage ToMimeMessage(this SendEmailRequest emailRequest)
    {
        MimeMessage mimeMessage = new();

        // TODO: Issue #3.
        mimeMessage.Sender = new MailboxAddress(
                emailRequest?.Email.Sender?.Name ?? string.Empty,
                emailRequest?.Email.Sender?.Address);

        // TODO: Issue #3.
        mimeMessage.From.Add(mimeMessage.Sender);

        mimeMessage.To.AddRange(emailRequest?.Email.Tos.ToInternetAddressList());
        mimeMessage.Cc.AddRange(emailRequest?.Email.Ccs?.ToInternetAddressList());
        mimeMessage.Bcc.AddRange(emailRequest?.Email.Bccs?.ToInternetAddressList());

        mimeMessage.Subject = emailRequest?.Subject();
        mimeMessage.Body = emailRequest?.ToMimeMessageBody();

        return mimeMessage;
    }



    /// <summary>
    /// This method converts the body of the SendEmailRequest object to a MimeKit.MailEntity object. 
    /// </summary>
    /// <returns>MimeKit.MimeEntity</returns>
    public static MimeEntity ToMimeMessageBody(this SendEmailRequest emailRequest)
    {
        ArgumentException.ThrowIfNullOrEmpty(emailRequest.Email.Body.Html);

        BodyBuilder bodyBuilder = new();

        bodyBuilder.HtmlBody = emailRequest.Email.Body.Html;
        bodyBuilder.TextBody = emailRequest.Email.Body.Text ?? string.Empty;

        return bodyBuilder.ToMessageBody();
    }



    /// <summary>
    /// This extention method injects string values into to the subject of the emailrequest
    /// depending on environment and debug- ore release mode. Nothing will be injects
    /// when the environment is Production end the release-mode Release.
    /// </summary>
    /// <returns>string</returns>
    public static string Subject(this SendEmailRequest emailRequest)
    {
        bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        bool isStaging = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging";

        bool isDebug = false;

#if DEBUG
        isDebug = true;
#endif

        var subject = emailRequest.Email.Subject;
        var status = string.Empty;

        List<string> statesList = [];

        if (isDebug)
        {
            statesList.Add("DEBUG");
        }

        if (isDevelopment)
        {
            statesList.Add("DEV");
        }

        if (isStaging)
        {
            statesList.Add("STAGE");
        }

        var states = string.Join(" - ", statesList);

        if (statesList.Count > 0)
        {
            return $"*** {states} *** {subject}";
        }

        return subject;
    }
}
