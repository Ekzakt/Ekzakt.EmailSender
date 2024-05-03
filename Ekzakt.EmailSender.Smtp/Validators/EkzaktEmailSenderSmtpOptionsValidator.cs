using Regexes = Ekzakt.Utilities.Validation.Regex;
using FluentValidation;
using System.Text.RegularExpressions;
using Ekzakt.EmailSender.Smtp.Configuration;

namespace Ekzakt.EmailSender.Smtp.Validators;

public class EkzaktEmailSenderSmtpOptionsValidator : AbstractValidator<EkzaktEmailSenderSmtpOptions>
{
    public EkzaktEmailSenderSmtpOptionsValidator()
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
