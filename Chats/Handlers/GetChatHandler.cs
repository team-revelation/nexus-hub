using System.Threading;
using System.Threading.Tasks;
using Chats.Queries;
using Contracts.Chats;
using MediatR;
using Types.Chats;

namespace Chats.Handlers
{
    public class GetChatHandler : IRequestHandler<GetChatQuery, Chat>
    {
        private readonly IChatService _chatService;

        public GetChatHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Chat> Handle(GetChatQuery request, CancellationToken cancellationToken)
        {
            return await _chatService.Get(request.Uuid);
        }
    }
}