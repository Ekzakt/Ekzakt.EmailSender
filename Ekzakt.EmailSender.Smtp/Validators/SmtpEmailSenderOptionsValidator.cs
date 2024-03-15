using Regexes = Ekzakt.Utilities.Validation.Regex;
using Ekzakt.EmailSender.Smtp.Configuration;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Ekzakt.EmailSender.Smtp.Validators;

[Obsolete("Use EkzaktSmtpEmailSenderOptionsValidator instead. This class will be removed in a future version.")]
internal class SmtpEmailSenderOptionsValidator : AbstractValidator<SmtpEmailSenderOptions>
{
    public SmtpEmailSenderOptionsValidator()
    {
        RuleFor(x => x.SenderAddress)
            .NotNull()
            .NotEmpty()
            .Must(senderaddress => 
                Regex.Match(senderaddress, Regexes.Internet.EMAIL_ADDRESS).Success);

        RuleFor(x => x.Username)
            .NotNull()
            .NotEmpty()
            .Length(1, 320);

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .Length(1, int.MaxValue);

        RuleFor(x => x.Host)
            .NotNull()
            .NotEmpty()
            .Must(host => 
                Regex.Match(host, Regexes.Internet.HOST_NAME).Success || 
                Regex.Match(host, Regexes.Internet.IPv4_ADDRESS).Success);

        RuleFor(x => x.Port)
            .InclusiveBetween(1, 65535);

    }
}
