using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace TradeCore.EventBus.RabbitMQ
{
    public class MqQueueHelper : IMessageBrokerHelper
    {
        private readonly MessageBrokerOptions _brokerOptions;

        public MqQueueHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _brokerOptions = Configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();
        }

        public IConfiguration Configuration { get; }

        public void QueueMessage(string messageText)
        {
            var factory = new ConnectionFactory
            {
                VirtualHost = _brokerOptions.VirtualHost,
                HostName = _brokerOptions.HostName,
                UserName = _brokerOptions.UserName,
                Password = _brokerOptions.Password,
                Port=_brokerOptions.Port
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(
                queue: "TradeCoreQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = JsonConvert.SerializeObject(messageText);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty, routingKey: "TradeCoreQueue", basicProperties: null, body: body);
        }
    }
}