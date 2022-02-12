using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands
{
    public class EditMessageCommand : IRequest<Chat>
    {
        public Guid Uuid { get; }
        public Guid ChatUuid { get; }
        public Message Message { get; }
        
        public EditMessageCommand(Guid uuid, Guid chatUuid, Message message)
        {
            Uuid = uuid;
            ChatUuid = chatUuid;
            Message = message;
        }
    }
}