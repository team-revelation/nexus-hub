using System;
using MediatR;
using Types.Chats;

namespace Chats.Notifications
{
    public class LeaveChatNotification : INotification
    {
        public Chat Chat { get; }
        public Guid UserUuid { get; }
        
        public LeaveChatNotification(Chat chat, Guid userUuid)
        {
            Chat = chat;
            UserUuid = userUuid;
        }
    }
}