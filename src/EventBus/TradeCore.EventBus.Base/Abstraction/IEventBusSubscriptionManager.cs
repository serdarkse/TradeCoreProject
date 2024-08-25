using TradeCore.EventBus.Base.Events;

namespace TradeCore.EventBus.Base.Abstraction
{
    //dışardan gönderilen subscriptionlar memory de tutulacak. bize gönderilen Integration ve IntegrationEventlar tutulacak. InMemory olacak
    //ancak bunları db'de tutmak da isteyebiliriz.
    //Bu değişikliği sağlayabilmek için burası kullanılacak.
    public interface IEventBusSubscriptionManager
    {
        bool IsEmpty { get; } //herhangi bir event dinleniyor mu kontrolü yapılır

        event EventHandler<string> OnEventRemoved; //event remove edildiği zaman içerde bir event oluşturulacak ve dışardan gelen unsubscribe methodu çalıştırğında bu event tetiklenecek
        void AddSubscription<T,TH>() where T : IntegrationEvent where TH: IIntegrationEventHandler<T>; 
        void RemoveSubscription<T,TH>() where T : IntegrationEvent where TH: IIntegrationEventHandler<T>;
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent; // dışardan event gönderildiğinde bizim o eventı dinleyip dinlemediğimi kontrol edilecek
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName); // event gönderildiğinde type bilgisini geri dönecek
        void Clear(); // bütün subscriptionlar clear edilebilecek
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent; // dışardan gelen bir eventın tüm subscriptionlarını  bütün handlerlarını geri dönebileceğiz.
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<T>(); // eventların isimleri uniq olacak ve keyleri olacak. routingkey olacak ve onu geri dönecek.
    }
}
