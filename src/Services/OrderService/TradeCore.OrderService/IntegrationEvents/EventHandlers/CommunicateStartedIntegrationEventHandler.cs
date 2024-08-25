using TradeCore.OrderService.IntegrationEvents.Events;
using TradeCore.EventBus.Base.Abstraction;
using TradeCore.EventBus.Base.Events;

namespace TradeCore.OrderService.IntegrationEvents.EventHandlers
{
    public class CommunicateStartedIntegrationEventHandler : IIntegrationEventHandler<CommunicateStartedIntegrationEvent>
    {
        private readonly IConfiguration configuration;
        private readonly IEventBus eventBus;

        public CommunicateStartedIntegrationEventHandler(IConfiguration _configuration, IEventBus _eventBus)
        {
            configuration = _configuration;
            eventBus = _eventBus;
        }

        public Task Handle(CommunicateStartedIntegrationEvent @event)
        {
            IntegrationEvent returnEvent = new CommunicateEmailIntegrationEvent(@event.AddressToSend, @event.Subject, @event.MessageBody);
            eventBus.Publish(returnEvent);
            return Task.CompletedTask;
        }
    }
}
