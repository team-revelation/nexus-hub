using System;
using System.Collections.Generic;
using MediatR;
using Types.Chats;

namespace Chats.Queries
{
    public class GetChatsQuery : IRequest<IEnumerable<Chat>>
    {
        public Guid Uuid { get; }
        
        public GetChatsQuery(Guid uuid)
        {
            Uuid = uuid;
        }
        
        public GetChatsQuery()
        {
            Uuid = Guid.Empty;
        }
    }
}