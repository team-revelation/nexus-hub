using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Chats.Commands.Messages;
using Contracts.Chats;
using MediatR;
using Types.Chats;

namespace Chats.Handlers.Messages
{
    public class VoteForOptionHandler : IRequestHandler<VoteForOptionCommand, Chat>
    {
        private readonly IChatService _chatService;
        
        public VoteForOptionHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<Chat> Handle(VoteForOptionCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null)
                throw new ArgumentException("This chat does not exist, please try again.");

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.MessageUuid);
            if (messageIndex is -1)
                throw new ArgumentException("This chat does not exist, please try again.");

            var message = chat.Messages[messageIndex];
            var poll = message.Polls.FirstOrDefault(poll => poll.Uuid == request.PollUuid);
            
            if (poll is null)
                throw new ArgumentException("This poll does not exist, please try again.");

            if (poll.Options.Count <= request.Vote)
                throw new ArgumentException("This is an invalid vote.");

            var hasVoted = false;
            foreach (var option in poll.Options)
            {
                if (option.Votes.Contains(request.UserUuid))
                {
                    hasVoted = true;
                    option.Votes.Remove(request.UserUuid);
                }
            }
            
            poll.Options[request.Vote].Votes.Add(request.UserUuid);
            if (!hasVoted) poll.Votes++;
            
            chat.Messages[messageIndex] = message;
            
            return await _chatService.Update(request.ChatUuid, chat);
        }
    }
}