using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public class EditMessageCommand : IRequest<Chat>
    {
        public Guid MessageUuid { get; }
        public Guid ChatUuid { get; }
        public Message Message { get; }
        
        public EditMessageCommand(Guid messageUuid, Guid chatUuid, Message message)
        {
            MessageUuid = messageUuid;
            ChatUuid = chatUuid;
            Message = message;
        }
    }
}