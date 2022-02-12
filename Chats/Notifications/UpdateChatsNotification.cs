using System.Collections.Generic;
using MediatR;
using Types.Chats;

namespace Chats.Notifications
{
    public class UpdateChatsNotification : INotification
    {
        public IEnumerable<Chat> Chats { get; }
        
        public UpdateChatsNotification(IEnumerable<Chat> chats)
        {
            Chats = chats;
        }
    }
}