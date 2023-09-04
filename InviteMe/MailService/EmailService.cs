using FluentEmail.Core;
using FluentEmail.Core.Models;
using InviteMe.Models;
using System.Reflection;

namespace InviteMe.MailService;

public class EmailService : IEmailService
{
    private readonly IFluentEmailFactory _fluentEmailFactory;

    public EmailService(IFluentEmailFactory fluentEmailFactory)
    {
        _fluentEmailFactory= fluentEmailFactory;
    }

    public async Task SendInvite(EmailMetadata emailMetadata, string template, Invite invite)
    {

        try
        {
            var email = await _fluentEmailFactory
                            .Create()
                            .To(emailMetadata.ToAddress)
                            .Subject(emailMetadata.Subject)
                            .UsingTemplateFromEmbedded(template, invite, GetType().Assembly) //If you want to use embedded resource Views.
                            .SendAsync();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
}
