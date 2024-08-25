using TradeCore.EventBus.Base.Events;

namespace TradeCore.OrderService.IntegrationEvents.Events
{
    public class CommunicateStartedIntegrationEvent : IntegrationEvent
    {
        public List<CommunicatEmailAttachment>? Attachments { get; set; }
        public string AddressToSend { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }

        public CommunicateStartedIntegrationEvent()
        {

        }

        public CommunicateStartedIntegrationEvent(string addressToSend, string subject, string messageBody)
        {
            AddressToSend = addressToSend;
            Subject = subject;
            MessageBody = messageBody;
        }
    }
}
