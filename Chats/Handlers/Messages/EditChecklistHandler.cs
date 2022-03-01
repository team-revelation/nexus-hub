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
    public class EditChecklistHandler : IRequestHandler<EditChecklistCommand, EditChecklistResponse>
    {
        private readonly IChatService _chatService;
        
        public EditChecklistHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<EditChecklistResponse> Handle(EditChecklistCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null) throw new ArgumentException("This chat does not exist, please try again.");

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.MessageUuid);
            if (messageIndex is -1) throw new ArgumentException("This message does not exist, please try again.");

            var message = chat.Messages[messageIndex];
            var checklist = message.Checklists.FirstOrDefault(c => c.Uuid == request.ChecklistUuid);
            if (checklist is null) throw new ArgumentException("This checklist does not exist, please try again.");

            checklist = request.Checklist with { Uuid = checklist.Uuid };
            var checklists = message.Checklists.Where(p => p.Uuid != request.ChecklistUuid).Append(checklist);
            chat.Messages[messageIndex] = message with
            {
                Checklists = checklists.ToList(),
                Edited = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            return new(await _chatService.Update(request.ChatUuid, chat), checklist);
        }
    }
}