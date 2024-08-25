namespace TradeCore.EventBus.Base
{
    public class EventBusConfig
    {
        public int ConnectionRetrycount { get; set; } = 5;
        public string DefaultTopicName { get; set; } = "TradeCoreBus";
        public string EventBusConnectionString { get; set; } = string.Empty;
        public string SubscriberClientAppName { get; set; } = string.Empty; //hangi servis yeni bir queue yaratacak. Kuyruklar oluşturulurken başına bu appname eklenecek. Aynı eventi birden fazla service kullanabilir. karmaşıklığın önüne geçileek
        public string EventNamePrefix { get; set; } = string.Empty;
        public string EventNameSuffix { get; set; } = "IntegrationEvent";
        public EventBusType EventBusType { get; set; } = EventBusType.RabbitMQ;
        public object Connection { get; set; } //tek bir MQ ya bağlı kalmamak adına object olarak tanımlanmıştır. Hnagi mq kullanılıyor ise ona cast edilecek.
        public bool DeleteEventPreffix => !String.IsNullOrEmpty(EventNamePrefix);
        public bool DeleteEventSuffix => !String.IsNullOrEmpty(EventNameSuffix);
    }

    public enum EventBusType
    {
        RabbitMQ,
        AzureServiceBus,
        AmazonMQBus,
        MSMQBus,
        KafkaBus
    }
}
