using MediatR;
using Types.Chats;
using Types.Notifications.Messages;

namespace Chats.Notifications
{
    public record SendMessageNotification : INotification
    {
        public Chat Chat { get; set; }
        public Message Message { get; set; }
        
        public SendMessageNotification(Chat chat, Message message)
        {
            Chat = chat;
            Message = message;
        }
        
        public SendMessageNotificationData AsData()
        {
            return new SendMessageNotificationData(Chat.Uuid, Message);
        }
    }
}