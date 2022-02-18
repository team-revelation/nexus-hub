using System;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Chats.Commands.Messages;
using Contracts.Chats;
using MediatR;
using Types.Chats;

namespace Chats.Handlers.Messages
{
    public class UnreadMessageHandler : IRequestHandler<UnreadMessageCommand, Chat>
    {
        private readonly IChatService _chatService;
        
        public UnreadMessageHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<Chat> Handle(UnreadMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.Uuid);
            if (messageIndex == -1)
                throw new ArgumentException("This message does not exist, please try again.");

            chat.Messages[messageIndex].ReadUsers.RemoveAll(read => read.UserUuid == request.UserUuid);
            return await _chatService.Update(request.ChatUuid, chat);
        }
    }
}