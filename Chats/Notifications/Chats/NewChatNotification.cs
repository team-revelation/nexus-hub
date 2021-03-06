using MediatR;
using Types.Chats;

namespace Chats.Notifications.Chats
{
    public class NewChatNotification : INotification
    {
        public Chat Chat { get; }
        
        public NewChatNotification(Chat chat)
        {
            Chat = chat;
        }
    }
}