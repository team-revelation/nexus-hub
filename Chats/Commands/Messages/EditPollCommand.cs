using System;
using MediatR;
using Types.Chats;

namespace Chats.Commands.Messages
{
    public class EditPollCommand : IRequest<Chat>
    {
        public Guid PollUuid { get; }
        public Guid MessageUuid { get; }
        public Guid ChatUuid { get; }
        public Poll Poll { get; }
        
        public EditPollCommand(Guid pollUuid, Guid messageUuid, Guid chatUuid, Poll poll)
        {
            PollUuid = pollUuid;
            MessageUuid = messageUuid;
            ChatUuid = chatUuid;
            Poll = poll;
        }
    }
}