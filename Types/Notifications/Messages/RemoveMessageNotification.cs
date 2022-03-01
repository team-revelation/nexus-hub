using System;

namespace Types.Notifications.Messages
{
    public record RemoveMessageNotificationData
    {
        public RemoveMessageNotificationData(Guid messageUuid, Guid chatUuid)
        {
            MessageUuid = messageUuid;
            ChatUuid = chatUuid;
        }
        
        public Guid ChatUuid { get; }
        public Guid MessageUuid { get; }
    }
}