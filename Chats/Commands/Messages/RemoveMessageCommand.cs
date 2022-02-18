using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public class RemoveMessageCommand : IRequest<Chat>
    {
        public Guid Uuid { get; }
        public Guid ChatUuid { get; }
        
        public RemoveMessageCommand(Guid chatUuid, Guid uuid)
        {
            ChatUuid = chatUuid;
            Uuid = uuid;
        }
    }
}