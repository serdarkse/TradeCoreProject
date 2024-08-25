using TradeCore.EventBus.Base.Events;

namespace TradeCore.NotificationService.IntegrationEvents.Events
{
    public class CommunicateEmailIntegrationEvent : IntegrationEvent
    {
        public List<CommunicatEmailAttachment>? Attachments { get; set; }
        public List<string> AddressToSend { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }

        public CommunicateEmailIntegrationEvent(List<string> addressToSend, string subject, string messageBody)
        {
            AddressToSend = addressToSend;
            Subject = subject;
            MessageBody = messageBody;
        }
        public class CommunicatEmailAttachment
        {
            public string Content { get; set; }
            public string FileName { get; set; }
        }
    }
}
