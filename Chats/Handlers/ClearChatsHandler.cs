using System.Threading;
using System.Threading.Tasks;
using Chats.Commands;
using Contracts.Chats;
using MediatR;

namespace Chats.Handlers
{
    public class ClearChatsHandler : IRequestHandler<ClearChatsCommand>
    {
        private readonly IChatService _chatService;
        
        public ClearChatsHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<Unit> Handle(ClearChatsCommand request, CancellationToken cancellationToken)
        {
            await _chatService.Clear();
            return Unit.Value;
        }
    }
}