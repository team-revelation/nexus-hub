using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public record EditMessageResponse(Chat Chat, Message Message);
    public class EditMessageCommand : IRequest<EditMessageResponse>
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