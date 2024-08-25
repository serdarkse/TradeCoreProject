using System.Net.Mail;

namespace TradeCore.NotificationService.Sender
{
    public interface IMailSender
    {
        bool SendMail(MailMessage mailMessage);
    }
}
