using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TradeCore.EventBus.RabbitMQ
{
    public class MqConsumerHelper : IMessageConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly MessageBrokerOptions _brokerOptions;

        public MqConsumerHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _brokerOptions = _configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();
        }

        public string GetQueue()
        {
            string result ="";
            try
            {
                var factory = new ConnectionFactory()
                {
                    VirtualHost = _brokerOptions.VirtualHost,
                    HostName = _brokerOptions.HostName,
                    Port = _brokerOptions.Port,
                    UserName = _brokerOptions.UserName,
                    Password = _brokerOptions.Password,
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(
                    queue: "TradeCoreQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, mq) =>
                {
                    var body = mq.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    result = message;
                    Console.WriteLine($"Message: {message}");
                };

                channel.BasicConsume(
                    queue: "TradeCoreQueue",
                    autoAck: true,
                    consumer: consumer);
                

                return result;
            }
            catch (Exception ex)
            {
               return result = ex.Message;
            }
        }
    }
}