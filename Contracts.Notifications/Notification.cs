using System.Linq;
using Types.Chats;

namespace Contracts.Notifications
{
    public record Notification
    {
        public Notification(NotificationType type, string color, FirebaseAdmin.Messaging.Notification obj)
        {
            Type = type;
            Color = color;
            Object = obj;
        }

        public Notification(string title, Message message)
        {
            Object = new FirebaseAdmin.Messaging.Notification
            {
                Title = title,
                Body = message.Content
            };
            
            if (message.Attachments.Any(a => a.Type == AttachmentType.Image))
                Object.ImageUrl = message.Attachments.First().Url;
        }

        public Notification(NotificationType type, string title, string description, string url)
        {
            Type = type;
            Object = new FirebaseAdmin.Messaging.Notification
            {
                Title = title, 
                Body = description,
                ImageUrl = url,
            };
        }
        
        public Notification(NotificationType type, string title, string description)
        {
            Type = type;
            Object = new FirebaseAdmin.Messaging.Notification
            {
                Title = title, 
                Body = description
            };
        }
        
        public Notification(string title, string description)
        {
            Object = new FirebaseAdmin.Messaging.Notification
            {
                Title = title, 
                Body = description
            };
        }

        public NotificationType Type { get; } = NotificationType.SendMessage;
        public string Color { get; } = "#a55eea";
        public FirebaseAdmin.Messaging.Notification Object { get; }
    }
}