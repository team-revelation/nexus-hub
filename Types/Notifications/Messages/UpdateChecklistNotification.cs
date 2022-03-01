using System;
using Types.Chats;

namespace Types.Notifications.Messages
{
    public record UpdateChecklistNotificationData
    {
        public UpdateChecklistNotificationData(Guid chatUuid, Guid messageUuid, Checklist checklist)
        {
            ChatUuid = chatUuid;
            MessageUuid = messageUuid;
            Checklist = checklist;
        }
        
        public Guid ChatUuid { get; }
        public Guid MessageUuid { get; }
        public Checklist Checklist { get; }
    }
}