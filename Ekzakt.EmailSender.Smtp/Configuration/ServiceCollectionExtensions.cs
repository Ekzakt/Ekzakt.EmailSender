using Ekzakt.EmailSender.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Ekzakt.EmailSender.Smtp.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSmtpEmailSender(this IServiceCollection services, Action<SmtpEmailSenderOptions> options)
    {
        services.Configure(options);

        services.AddScoped<IEmailSenderService, SmtpEmailSenderService>();

        return services;
    }


    public static IServiceCollection AddSmtpEmailSender(this IServiceCollection services, string configSectionPath)
    {
        services.AddOptions<SmtpEmailSenderOptions>()
            .BindConfiguration(configSectionPath);

        services.AddScoped<IEmailSenderService, SmtpEmailSenderService>();

        return services;
    }

}
