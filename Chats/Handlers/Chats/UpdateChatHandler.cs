using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Chats.Commands.Chats;
using Contracts.Chats;
using MediatR;
using Types.Chats;

namespace Chats.Handlers.Chats
{
    public class UpdateChatHandler : IRequestHandler<UpdateChatCommand, Chat>
    {
        private readonly IChatService _chatService;
        
        public UpdateChatHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<Chat> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await _chatService.Get(request.Uuid);
            var newChat = chat with
            {
                Name = request.Chat.Name ?? chat.Name,
                Avatar = request.Chat.Avatar ?? chat.Avatar,
                Roles = request.Chat.Roles ?? chat.Roles,
                Members = request.Chat.Members ?? chat.Members,
            };
            
            return await _chatService.Update(request.Uuid, newChat);
        }
    }
}