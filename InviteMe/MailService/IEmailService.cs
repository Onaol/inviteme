using InviteMe.Models;

namespace InviteMe.MailService
{
    public interface IEmailService
    {
        Task SendInvite(EmailMetadata emailMetadata, string template, Invite invite);
    }
}
