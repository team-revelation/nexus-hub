using System;
using MediatR;

namespace Chats.Commands.Chats
{
    public class RemoveChatCommand : IRequest
    {
        public Guid Uuid { get; }
        
        public RemoveChatCommand(Guid uuid)
        {
            Uuid = uuid;
        }
    }
}