namespace TradeCore.EventBus.Base
{
    //dışardan gönderilen verilerin içerde tutulması için kullanılacak
    public class SubscriptionInfo
    {
        public Type HandlerType { get; }

        public SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        }
        public static SubscriptionInfo Typed(Type handlerType)
        {
            return new SubscriptionInfo(handlerType);
        }
    }
}