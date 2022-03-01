using System;
using Types.Chats;

namespace Types.Notifications.Messages
{
    public record UpdateMessageNotificationData
    {
        public UpdateMessageNotificationData(Guid chatUuid, Message message)
        {
            ChatUuid = chatUuid;
            Message = message;
        }
        
        public Guid ChatUuid { get; }
        public Message Message { get; }
    }
}