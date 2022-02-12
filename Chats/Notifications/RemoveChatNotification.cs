using MediatR;
using Types.Chats;

namespace Chats.Notifications
{
    public class RemoveChatNotification : INotification
    {
        public Chat Chat { get; }
        
        public RemoveChatNotification(Chat chat)
        {
            Chat = chat;
        }
    }
}