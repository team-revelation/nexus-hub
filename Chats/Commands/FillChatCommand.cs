using MediatR;
using Types.Chats;

namespace Chats.Commands
{
    public class FillChatCommand : IRequest<Chat>
    {
        public Chat Chat { get; }
        
        public FillChatCommand(Chat chat)
        {
            Chat = chat;
        }   
    }
}