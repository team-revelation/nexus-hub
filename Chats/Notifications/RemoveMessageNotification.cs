using System;
using MediatR;
using Types.Chats;
using Types.Notifications.Messages;

namespace Chats.Notifications
{
    public record RemoveMessageNotification : INotification
    {
        public Chat Chat { get; set; }
        public Guid MessageUuid { get; set; }
        
        public RemoveMessageNotification(Chat chat, Guid messageUuid)
        {
            Chat = chat;
            MessageUuid = messageUuid;
        }

        public RemoveMessageNotificationData AsData()
        {
            return new RemoveMessageNotificationData(MessageUuid, Chat.Uuid);
        }
    }
}