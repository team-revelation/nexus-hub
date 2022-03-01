using System;
using MediatR;
using Types.Chats;
using Types.Notifications.Messages;

namespace Chats.Notifications
{
    public record UnreadMessageNotification : INotification
    {
        public Chat Chat { get; set; }
        public Guid MessageUuid { get; set; }
        public Guid UserUuid { get; set; }
        
        public UnreadMessageNotification(Guid userUuid, Guid messageUuid, Chat chat)
        {
            UserUuid = userUuid;
            MessageUuid = messageUuid;
            Chat = chat;
        }
        
        public UnreadMessageNotificationData AsData()
        {
            return new UnreadMessageNotificationData(Chat.Uuid, MessageUuid, UserUuid);
        }
    }
}