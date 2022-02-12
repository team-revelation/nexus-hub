using MediatR;
using Types.Chats;

namespace Chats.Notifications
{
    public class UpdateChatNotification : INotification
    {
        public Chat Chat { get; }
        
        public UpdateChatNotification(Chat chat)
        {
            Chat = chat;
        }
    }
}