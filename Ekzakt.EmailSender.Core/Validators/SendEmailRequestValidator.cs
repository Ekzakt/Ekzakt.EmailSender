using Ekzakt.EmailSender.Core.Models;
using FluentValidation;

namespace Ekzakt.EmailSender.Core.Validators;

public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
{
    public SendEmailRequestValidator()
    {
        RuleFor(x => x.Body.Html)
            .NotNull().NotEmpty();

        RuleFor(x => x.Sender)
            .SetValidator(new EmailAddressValidator());

        RuleFor(x => x.Tos.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage("At least one destination address (Tos) must be set.");

        RuleForEach(x => x.Tos)
            .NotNull()
            .NotEmpty()
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
