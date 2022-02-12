using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands
{
    public class SendMessageCommand : IRequest<Chat>
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