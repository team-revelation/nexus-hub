using System;
using MediatR;
using Types.Chats;
using Types.Notifications.Messages;

namespace Chats.Notifications
{
    public record UpdateMessageNotification : INotification
    {
        public Chat Chat { get; set; }
        public Message Message { get; set; }
        
        public UpdateMessageNotification(Chat chat, Message message)
        {
            Chat = chat;
            Message = message;
        }
        
        public UpdateMessageNotificationData AsData()
        {
            return new UpdateMessageNotificationData(Chat.Uuid, Message);
        }
    }
}