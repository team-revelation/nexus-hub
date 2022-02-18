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
    public class EditPollHandler : IRequestHandler<EditPollCommand, Chat>
    {
        private readonly IChatService _chatService;
        
        public EditPollHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<Chat> Handle(EditPollCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null) throw new ArgumentException("This chat does not exist, please try again.");

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.MessageUuid);
            if (messageIndex is -1) throw new ArgumentException("This chat does not exist, please try again.");

            var message = chat.Messages[messageIndex];
            var poll = message.Polls.FirstOrDefault(c => c.Uuid == request.PollUuid);
            if (poll is null) throw new ArgumentException("This checklist does not exist, please try again.");

            poll = request.Poll with
            {
                Uuid = poll.Uuid,
                Votes = request.Poll.Options.SelectMany(o => o.Votes).Count()
            };
            
            var polls = message.Polls.Where(p => p.Uuid != poll.Uuid).Append(poll);
            chat.Messages[messageIndex] = message with
            {
                Polls = polls.ToList(),
                Edited = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            return await _chatService.Update(request.ChatUuid, chat);
        }
    }
}