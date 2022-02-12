using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands
{
    public class UpdateChatCommand : IRequest<Chat>
    {
        public Guid Uuid { get; }
        public Chat Chat { get; }
        
        public UpdateChatCommand(Guid uuid, Chat chat)
        {
            Uuid = uuid;
            Chat = chat;
        }
    }
}