using System.Net.Mail;
using System.Text;

namespace TradeCore.OrderService.Utilities.Mail
{
    public class MailManager : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(EmailMessage emailMessage)
        {
            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(_configuration.GetSection("EmailConfiguration").GetSection("SmtpPort").Value);
            client.Host = _configuration.GetSection("EmailConfiguration").GetSection("SmtpServer").Value;
            client.EnableSsl = false;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            MailMessage message = new MailMessage();
            message.From = emailMessage.FromAddresses.Select(x => new MailAddress(x.Address)).FirstOrDefault();


            var toUsers = emailMessage.ToAddresses.Select(x => new MailAddress(x.Address));
            foreach (var item in toUsers)
            {
                message.To.Add(item);
            }
            message.Subject = emailMessage.Subject;

            string htmlString = @"<html>
                      <body>
                      <p>Merhaba</p>
                      <p> " + emailMessage.Content + @" </p>
                      </body>
                      </html>
                     ";

            message.Body = htmlString;

            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(message);

        }
    }
}