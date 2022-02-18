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
    public class EditMessageHandler : IRequestHandler<EditMessageCommand, Chat>
    {
        private readonly IChatService _chatService;
        
        public EditMessageHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<Chat> Handle(EditMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            if (chat is null)
                throw new ArgumentException("This chat does not exist, please try again.");

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.MessageUuid);
            if (messageIndex is -1)
                throw new ArgumentException("This chat does not exist, please try again.");

            var message = chat.Messages[messageIndex];
            chat.Messages[messageIndex] = message with
            {
                Embeds = request.Message.Embeds,
                Attachments = request.Message.Attachments,
                Content = request.Message.Content,
                Edited = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            return await _chatService.Update(request.ChatUuid, chat);
        }
    }
}