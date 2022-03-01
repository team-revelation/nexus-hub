using System;
using MediatR;
using Types.Chats;
using Types.Notifications.Messages;

namespace Chats.Notifications
{
    public record UpdateChecklistNotification : INotification
    {
        public Chat Chat { get; set; }
        public Guid MessageUuid { get; set; }
        public Checklist Checklist { get; set; }
        
        public UpdateChecklistNotification(Chat chat, Guid messageUuid, Checklist checklist)
        {
            Chat = chat;
            MessageUuid = messageUuid;
            Checklist = checklist;
        }
        
        public UpdateChecklistNotificationData AsData()
        {
            return new UpdateChecklistNotificationData(Chat.Uuid, MessageUuid, Checklist);
        }
    }
}