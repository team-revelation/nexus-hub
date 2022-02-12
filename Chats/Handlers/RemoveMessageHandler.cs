using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Contracts.Chats;
using MediatR;
using Types.Chats;

namespace Chats.Handlers
{
    public class RemoveMessageHandler : IRequestHandler<RemoveMessageCommand, Chat>
    {
        private readonly IChatService _chatService;
        
        public RemoveMessageHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<Chat> Handle(RemoveMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null || !chat.Messages.Exists(message => message.Uuid == request.Uuid))
                throw new ArgumentException("This chat does not exist, please try again.");

            chat.Messages = chat.Messages.Where(message => message.Uuid != request.Uuid).ToList();
            return await _chatService.Update(chat.Uuid, chat);
        }
    }
}