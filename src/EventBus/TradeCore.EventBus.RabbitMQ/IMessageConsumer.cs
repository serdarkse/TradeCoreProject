namespace TradeCore.EventBus.RabbitMQ
{
    public interface IMessageConsumer
    {
        string GetQueue();
    }
}