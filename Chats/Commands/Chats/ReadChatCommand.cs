using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Chats
{
    public class ReadChatCommand : IRequest<Chat>
    {
        public Guid Uuid { get; }
        public Guid UserUuid { get; }
        
        public ReadChatCommand(Guid uuid, Guid userUuid)
        {
            Uuid = uuid;
            UserUuid = userUuid;
        }
    }
}