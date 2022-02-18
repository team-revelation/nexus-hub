using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Chats.Commands.Chats;
using Contracts.Chats;
using MediatR;
using Types.Chats;

namespace Chats.Handlers.Chats
{
    public class ReadChatHandler : IRequestHandler<ReadChatCommand, Chat>
    {
        private readonly IChatService _chatService;
        
        public ReadChatHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<Chat> Handle(ReadChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.Uuid);
            if (chat is null)
                throw new ArgumentException("This chat does not exist, please try again.");

            var hasChanged = false;
            foreach (var message in chat.Messages)
            {
                if (message.ReadUsers.Any(read => read.UserUuid == request.UserUuid)) continue;

                message.ReadUsers.Add(new (chat.Members.First(member => member.Uuid == request.UserUuid)));
                hasChanged = true;
            }

            return hasChanged ? await _chatService.Update(request.Uuid, chat) : chat;
        }
    }
}