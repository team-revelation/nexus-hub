using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Other
{
    public class SetTypingCommand : IRequest<Status>
    {
        public Guid UserUuid { get; }
        public Chat Chat { get; }
        public bool IsTyping { get; }

        public SetTypingCommand(Guid userUuid, Chat chat, bool isTyping)
        {
            UserUuid = userUuid;
            Chat = chat;
            IsTyping = isTyping;
        }
    }
}