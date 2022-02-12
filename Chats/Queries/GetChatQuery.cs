using System;
using MediatR;
using Types.Chats;

namespace Chats.Queries
{
    public class GetChatQuery : IRequest<Chat>
    {
        public Guid Uuid { get; }

        public GetChatQuery(Guid uuid)
        {
            Uuid = uuid;
        }
    }
}