using System.Net.Mail;

namespace TradeCore.NotificationService.Sender
{
    public class MailSender : IMailSender
    {
        private readonly ILogger<MailSender> _logger;

        public MailSender(ILogger<MailSender> logger)
        {
            _logger = logger;
        }
        private bool CreateMail(MailMessage mailMessage)
        {
            try
            {
                return SmtpClient(mailMessage);

            }
            catch (Exception)
            {
                _logger .LogError("Error while creating mail.");
                throw;
            }
        }
        public bool SendMail(MailMessage mailMessage)
        {
            CreateMail(mailMessage);
            return true;
        }

        private bool SmtpClient(MailMessage mailMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 25,
                EnableSsl = false,

            };
            smtpClient.Send(mailMessage);
            return true;
        }
    }
}
