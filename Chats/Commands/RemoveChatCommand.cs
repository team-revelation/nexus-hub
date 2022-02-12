using System;
using MediatR;

namespace Chats.Commands
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