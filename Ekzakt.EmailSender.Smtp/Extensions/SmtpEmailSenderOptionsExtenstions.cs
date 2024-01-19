using Ekzakt.EmailSender.Smtp.Configuration;
using FluentValidation;

namespace Ekzakt.EmailSender.Smtp.Extensions;

public static class SmtpEmailSenderOptionsExtenstions
{
    public static void Validate(this SmtpEmailSenderOptions smtpEmailSenderOptions, IValidator<SmtpEmailSenderOptions> optionsValidator)
    {
        var validationResult = optionsValidator.Validate(smtpEmailSenderOptions);

        if (!validationResult.IsValid)
        {
            var failure = validationResult.Errors.FirstOrDefault();

            var message = 
                $"Invalidated {nameof(SmtpEmailSenderOptions)}. " +
                $"Property {failure?.PropertyName} has an invalid " +
                $"value of {failure?.AttemptedValue}.";

            throw new InvalidOperationException(message);
        }
    }
}
