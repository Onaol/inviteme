using FluentEmail.Core;
using FluentEmail.Razor;
using InviteMe.MailService;
using System.Reflection;

namespace InviteMe.Extensions;

public static class EmailExtensions
{
    public static void AddFluentEmail(this IServiceCollection services,
     ConfigurationManager configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        var defaultFromEmail = emailSettings["DefaultFromEmail"];
        var host = emailSettings["SMTPSetting:Host"];
        var port = emailSettings.GetValue<int>("Port");
        var userName = emailSettings["UserName"];
        var password = emailSettings["Password"];
        services.AddFluentEmail(defaultFromEmail).AddRazorRenderer(typeof(Program))
            .AddSmtpSender(host, port, userName, password);        

        services.AddScoped<IEmailService, EmailService>();
    }
}
