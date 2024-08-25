using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using TradeCore.EventBus.Base;
using TradeCore.EventBus.Base.Events;

namespace TradeCore.EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        RabbitMQPersistentConnection persistentConnection;
        private readonly IConnectionFactory connectionFactory;
        private readonly IModel consumerChannel;

        public EventBusRabbitMQ(EventBusConfig config, IServiceProvider serviceProvider, IConfiguration configuration) : base(config,serviceProvider) 
        {
            var _endpoints = new List<AmqpTcpEndpoint>();
            var _brokerOptions = configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOptions>();

            if (config.Connection !=null)
            {
                connectionFactory = new ConnectionFactory()
                {
                    VirtualHost = _brokerOptions.VirtualHost,
                    UserName = _brokerOptions.UserName,
                    Password = _brokerOptions.Password,
                    Port = _brokerOptions.Port
                };
                var hostArray = _brokerOptions.HostName.Split(',');
                foreach (var item in hostArray)
                    _endpoints.Add(new AmqpTcpEndpoint(item, _brokerOptions.Port));
            }
            else
                connectionFactory = new ConnectionFactory();

            persistentConnection = new RabbitMQPersistentConnection(connectionFactory, _endpoints,config.ConnectionRetrycount);

            consumerChannel = CreateConsumerChannel();

            SubsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            consumerChannel.QueueUnbind(queue:eventName,
                exchange: eventBusConfig.DefaultTopicName,
                routingKey:eventName);

            if (SubsManager.IsEmpty)
            {
                consumerChannel.Close(); 
            }

        }

        public override void Publish(IntegrationEvent @event)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                   .Or<SocketException>()
                   .WaitAndRetry(eventBusConfig.ConnectionRetrycount, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)), (ex, time) =>
                   {

                   });
            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);

            consumerChannel.ExchangeDeclare(exchange: eventBusConfig.DefaultTopicName, type:"direct");

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);     

            policy.Execute(() =>
            {
                var properties = consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                //consumerChannel.QueueDeclare(queue:GetSubName(eventName),
                //    durable:true,
                //    exclusive:false,
                //    autoDelete:false,
                //    arguments:null);

                //consumerChannel.QueueBind(queue:GetSubName(eventName),
                //    exchange: eventBusConfig.DefaultTopicName,
                //    routingKey:eventName);

                consumerChannel.BasicPublish(exchange: eventBusConfig.DefaultTopicName,
                    routingKey:eventName,
                    mandatory:true,
                    basicProperties:properties,
                    body:body);

            });



        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);

            if (!SubsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!persistentConnection.IsConnected)
                {
                    persistentConnection.TryConnect();
                }
                consumerChannel.QueueDeclare(queue:GetSubName(eventName), 
                    durable:true,
                    exclusive:false,
                    autoDelete:false,
                    arguments:null);

                consumerChannel.QueueBind(queue: GetSubName(eventName),
                    exchange: eventBusConfig.DefaultTopicName,
                    routingKey: eventName);
            }

            SubsManager.AddSubscription<T, TH>();
            StartBasicConsume(eventName);
        }

        public override void UnSubscribe<T, TH>()
        {
            SubsManager.RemoveSubscription<T, TH>();    
        }

        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: eventBusConfig.DefaultTopicName,type:"direct");

            return channel;
        }

        private void StartBasicConsume(string eventName)
        {
            if (consumerChannel !=null)
            {
                var consumer = new EventingBasicConsumer(consumerChannel);

                consumer.Received += Consumer_Received;

                consumerChannel.BasicConsume(
                    queue:GetSubName(eventName),
                    autoAck:false,
                    consumer:consumer);

            }
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName,message);
            }
            catch (Exception ex)
            {
                //logging
            }
            consumerChannel.BasicAck(eventArgs.DeliveryTag,multiple:false);
        }
    }
}
