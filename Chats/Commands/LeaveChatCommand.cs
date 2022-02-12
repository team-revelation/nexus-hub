using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands
{
    public class LeaveChatCommand : IRequest<Chat>
    {
        public Guid Uuid { get; }
        public Guid UserUuid { get; }
        
        public LeaveChatCommand(Guid uuid, Guid userUuid)
        {
            Uuid = uuid;
            UserUuid = userUuid;
        }
    }
}