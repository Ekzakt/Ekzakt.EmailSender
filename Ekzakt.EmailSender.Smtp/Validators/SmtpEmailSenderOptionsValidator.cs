using Ekzakt.EmailSender.Smtp.Configuration;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Ekzakt.EmailSender.Smtp.Validators;

internal class SmtpEmailSenderOptionsValidator : AbstractValidator<SmtpEmailSenderOptions>
{
    public SmtpEmailSenderOptionsValidator()
    {
        RuleFor(x => x.FromAddress)
            .EmailAddress();

        RuleFor(x => x.UserName)
            .NotNull()
            .NotEmpty()
            .Length(1, 320);

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .Length(1, int.MaxValue);

        RuleFor(x => x.Host).Must(host => 
            Regex.Match(host, RegexConstants.HOST_NAME).Success || 
            Regex.Match(host, RegexConstants.IPv4_ADDRESS).Success);

        RuleFor(x => x.Port)
            .InclusiveBetween(1, 65535);

    }
}
