using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Chats.Commands.Chats;
using Contracts.Chats;
using MediatR;

namespace Chats.Handlers.Chats
{
    public class RemoveChatHandler: IRequestHandler<RemoveChatCommand>
    {
        private readonly IChatService _chatService;
        
        public RemoveChatHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Unit> Handle(RemoveChatCommand request, CancellationToken cancellationToken)
        {
            await _chatService.Delete(request.Uuid);
            return Unit.Value;
        }
    }
}