using System;
using Types.Chats;

namespace Types.Notifications.Messages
{
    public record UnreadMessageNotificationData
    {
        public UnreadMessageNotificationData(Guid chatUuid, Guid messageUuid, Guid userUuid)
        {
            ChatUuid = chatUuid;
            MessageUuid = messageUuid;
            UserUuid = userUuid;
        }
        
        public Guid ChatUuid { get; }
        public Guid MessageUuid { get; }
        public Guid UserUuid { get; }
    }
}