using TradeCore.EventBus.Base.Events;

namespace TradeCore.EventBus.Base.Abstraction
{
    //birden fazla event bus için bu yapı kuruluyor. şuan sadece rabbit için var yarın AzureServiceBus eklenebilir.
    //her biri burayı kullanacak
    //subscribe ordercreated veya emailsend için subscribe verileceği zaman kullanılacak
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
        void Subscribe<T,TH>() where T: IntegrationEvent where TH:IIntegrationEventHandler<T>;
        void UnSubscribe<T,TH>() where T: IntegrationEvent where TH:IIntegrationEventHandler<T>;
    }
}
