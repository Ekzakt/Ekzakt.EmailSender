using Ekzakt.EmailSender.Core.Models;
using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace Ekzakt.EmailSender.Core.Validators;

public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(x => x.Body.Html)
            .NotEmpty().NotNull();

        RuleFor(x => x.From)
            .SetValidator(new EmailAddressValidator());

        RuleForEach(x => x.Tos)
            .SetValidator(new EmailAddressValidator());

        RuleForEach(x => x.Ccs)
            .SetValidator(new EmailAddressValidator());

        RuleForEach(x => x.Bccs)
            .SetValidator(new EmailAddressValidator());

        RuleFor(x => x.Subject)
            .NotNull().NotEmpty();

        RuleFor(x => x.Body)
            .SetValidator(new EmailBodyValidator());
    }
}
