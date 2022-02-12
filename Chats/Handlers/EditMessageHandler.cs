using System;
using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Contracts.Chats;
using Google.Cloud.Firestore;
using MediatR;
using Types.Chats;

namespace Chats.Handlers
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

            var messageIndex = chat.Messages.FindIndex(message => message.Uuid == request.Uuid);
            if (messageIndex is -1)
                throw new ArgumentException("This chat does not exist, please try again.");

            var message = chat.Messages[messageIndex];
            chat.Messages[messageIndex] = message with
            {
                Embeds = request.Message.Embeds,
                Attachments = request.Message.Attachments,
                Polls = request.Message.Polls,
                Checklists = request.Message.Checklists,
                Content = request.Message.Content,
                Edited = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            return await _chatService.Update(request.ChatUuid, chat);
        }
    }
}