using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands
{
    public class ReactToMessageCommand : IRequest<Chat>
    {
        public Guid UserUuid { get; }
        public Guid ChatUuid { get; }
        public Guid MessageUuid { get; }
        public Emoticon Emoji { get; }
        
        public ReactToMessageCommand(Guid userUuid, Guid messageUuid, Guid chatUuid, Emoticon emoji)
        {
            UserUuid = userUuid;
            MessageUuid = messageUuid;
            ChatUuid = chatUuid;
            Emoji = emoji;
        }
    }
}