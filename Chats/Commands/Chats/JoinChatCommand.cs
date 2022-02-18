using System;
using System.Collections.Generic;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Chats
{
    public class JoinChatCommand : IRequest<Chat>
    {
        public Guid Uuid { get; }
        public List<Guid> UserUuids { get; }
        
        public JoinChatCommand(List<Guid> userUuids, Guid uuid)
        {
            UserUuids = userUuids;
            Uuid = uuid;
        }
    }
}