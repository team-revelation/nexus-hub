using System.Collections.Generic;
using MediatR;
using Types.Chats;

namespace Chats.Commands
{
    public class FillChatsCommand : IRequest<IEnumerable<Chat>>
    {
        public IEnumerable<Chat> Chats { get; }
        
        public FillChatsCommand(IEnumerable<Chat> chats)
        {
            Chats = chats;
        }   
    }
}