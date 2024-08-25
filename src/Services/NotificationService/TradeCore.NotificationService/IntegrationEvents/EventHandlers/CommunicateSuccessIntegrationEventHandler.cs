using System.Net.Mail;
using System.Text;
using TradeCore.EventBus.Base.Abstraction;
using TradeCore.NotificationService.IntegrationEvents.Events;

namespace TradeCore.NotificationService.IntegrationEvents.EventHandlers
{
    public class CommunicateSuccessIntegrationEventHandler : IIntegrationEventHandler<CommunicateEmailIntegrationEvent>
    {
        private readonly IConfiguration _configuration;

        public CommunicateSuccessIntegrationEventHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task Handle(CommunicateEmailIntegrationEvent @event)
        {
            #region
            //mail gönderim işlemeri
            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(_configuration.GetSection("EmailConfiguration").GetSection("SmtpPort").Value);
            client.Host = _configuration.GetSection("EmailConfiguration").GetSection("SmtpServer").Value;
            client.EnableSsl = false;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetSection("EmailConfiguration").GetSection("SenderEmail").Value);

            var toUsers = @event.AddressToSend;


            foreach (var item in toUsers)
            {
                if (!message.To.Any(a => a.Address == item))
                    message.To.Add(new MailAddress(item));
            }

            message.Subject = @event.Subject;
            message.Body = @event.MessageBody;
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(message);
            #endregion

            #region
            // MailMessage nesnesini a.txt dosyasına yazma
            var messageContent = new StringBuilder();
            messageContent.AppendLine($"From: {message.From}");
            messageContent.AppendLine("To: " + string.Join(", ", message.To.Select(to => to.Address)));
            messageContent.AppendLine($"Subject: {message.Subject}");
            messageContent.AppendLine("Body:");
            messageContent.AppendLine(message.Body);

            File.AppendAllText("a.txt", $"{message}");
            #endregion

            return Task.CompletedTask;
        }

    }
}
