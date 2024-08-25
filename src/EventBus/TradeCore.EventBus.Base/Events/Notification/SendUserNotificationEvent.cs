using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCore.EventBus.Base.Events.Notification
{
    public class SendUserNotificationEvent : IntegrationEvent
    {
        public NotificationType NotificationType { get; set; }
        public UserNotificationAttachment? Attachment { get; set; }
        public string AddressToSend { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
    }

    public class UserNotificationAttachment
    {
        public string Content { get; set; }
        public string FileName { get; set; }
    }

    public enum NotificationType
    {
        EMAIL = 1, SMS = 2
    }
}
