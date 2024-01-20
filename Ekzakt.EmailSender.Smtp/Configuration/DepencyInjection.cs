using Ekzakt.EmailSender.Core.Contracts;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Core.Validators;
using Ekzakt.EmailSender.Smtp.Services;
using Ekzakt.EmailSender.Smtp.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ekzakt.EmailSender.Smtp.Configuration;

public static class DepencyInjection
{
    public static IServiceCollection AddSmtpEmailSender(this IServiceCollection services, Action<SmtpEmailSenderOptions> options)
    {
        services.Configure(options);

        services.AddSmtpEmailSender();

        return services;
    }


    public static IServiceCollection AddSmtpEmailSender(this IServiceCollection services, string? configSectionPath = null)
    {
        configSectionPath ??= SmtpEmailSenderOptions.OptionsName;

        services
            .AddOptions<SmtpEmailSenderOptions>()
            .BindConfiguration(configSectionPath);

        services.AddSmtpEmailSender(); 
        
        return services;
    }




    #region Helpers

    private static IServiceCollection AddSmtpEmailSender(this IServiceCollection services)
    {
        services.AddScoped<IValidator<SmtpEmailSenderOptions>, SmtpEmailSenderOptionsValidator>();
        services.AddScoped<IValidator<SendEmailRequest>, SendEmailRequestValidator>();
        services.AddScoped<IValidator<EmailBody>, EmailBodyValidator>();
        services.AddScoped<IValidator<EmailAddress>, EmailAddressValidator>();

        services.AddScoped<IEmailSenderService, SmtpEmailSenderService>();

        return services;
    }

    #endregion Helpers
}
