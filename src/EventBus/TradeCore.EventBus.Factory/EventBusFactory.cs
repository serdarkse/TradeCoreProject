using Microsoft.Extensions.Configuration;
using TradeCore.EventBus.Base;
using TradeCore.EventBus.Base.Abstraction;
using TradeCore.EventBus.RabbitMQ;

namespace TradeCore.EventBus.Factory
{
    public static class EventBusFactory 
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            return config.EventBusType switch
            {
                EventBusType.RabbitMQ => new EventBusRabbitMQ(config,serviceProvider, configuration),
                _=> new EventBusRabbitMQ(config, serviceProvider, configuration),
            } ;
        }

    }
}