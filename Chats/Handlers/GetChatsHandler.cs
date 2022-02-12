using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chats.Queries;
using Contracts.Chats;
using MediatR;
using Types.Chats;

namespace Chats.Handlers
{
    public class GetChatsHandler : IRequestHandler<GetChatsQuery, IEnumerable<Chat>>
    {
        private readonly IChatService _chatService;

        public GetChatsHandler(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<IEnumerable<Chat>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
        {
            return request.Uuid != Guid.Empty ? await _chatService.All(request.Uuid) : await _chatService.All();
        }
    }
}