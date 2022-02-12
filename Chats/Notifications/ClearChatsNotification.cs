using System.Collections.Generic;
using MediatR;
using Types.Chats;

namespace Chats.Notifications
{
    public class ClearChatsNotification : INotification
    {
        public List<Chat> Chats { get; set; }
    }
}