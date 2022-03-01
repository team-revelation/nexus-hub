using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public record SendMessageResponse(Chat Chat, Message Message);
    public class SendMessageCommand : IRequest<SendMessageResponse>
    {
        public Message Message { get; }
        public Guid ChatUuid { get; }
        
        public SendMessageCommand(Guid chatUuid, Message message)
        {
            ChatUuid = chatUuid;
            Message = message;
        }
    }
}