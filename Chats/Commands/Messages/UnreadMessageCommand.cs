using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public class UnreadMessageCommand : IRequest<Chat>
    {
        public Guid Uuid { get; }
        public Guid ChatUuid { get; }
        public Guid UserUuid { get; }
        
        public UnreadMessageCommand(Guid uuid, Guid userUuid, Guid chatUuid)
        {
            Uuid = uuid;
            UserUuid = userUuid;
            ChatUuid = chatUuid;
        }
    }
}