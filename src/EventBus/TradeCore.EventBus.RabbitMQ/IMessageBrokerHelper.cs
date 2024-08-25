namespace TradeCore.EventBus.RabbitMQ
{
    public interface IMessageBrokerHelper
    {
        void QueueMessage(string messageText);
    }
}