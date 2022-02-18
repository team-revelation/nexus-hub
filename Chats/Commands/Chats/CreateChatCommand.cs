using System;
using System.Collections.Generic;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Chats
{
    public class CreateChatRequest
    {
        public Chat Chat { get; init; }
        public IEnumerable<Guid> MemberUuids { get; init; }
        
        public CreateChatRequest() { }
    }
    
    public class CreateChatCommand : IRequest<Chat>
    {
        public Chat Chat { get; }
        public IEnumerable<Guid> MemberUuids { get; }
        
        public CreateChatCommand(Chat chat)
        {
            Chat = chat;
        }
        
        public CreateChatCommand(Chat chat, IEnumerable<Guid> memberUuids)
        {
            MemberUuids = memberUuids;
            Chat = chat;
        }
    }
}