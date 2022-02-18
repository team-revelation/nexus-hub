using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public class VoteForOptionCommand : IRequest<Chat>
    {
        public Guid UserUuid { get; }
        public Guid ChatUuid { get; }
        public Guid MessageUuid { get; }
        public Guid PollUuid { get; }
        public int Vote { get; }
        
        public VoteForOptionCommand(Guid userUuid, Guid chatUuid, Guid messageUuid, Guid pollUuid, int vote)
        {
            UserUuid = userUuid;
            ChatUuid = chatUuid;
            MessageUuid = messageUuid;
            PollUuid = pollUuid;
            Vote = vote;
        }   
    }
}