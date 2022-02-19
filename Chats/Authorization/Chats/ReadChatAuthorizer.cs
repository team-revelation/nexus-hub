using System.Linq;
using Chats.Commands.Chats;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Chats
{
    public class ReadChatAuthorizer : AbstractRequestAuthorizer<ReadChatCommand>
    {
        private readonly IChatService _chatService;
        
        public ReadChatAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(ReadChatCommand request)
        {
            var chat = await _chatService.Get(request.Uuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var isSameUser = request.Uuid == user.Uuid;
                    var isMember = chat.Members.Any(member => member.Uuid == user.Uuid);
                    return isMember && isSameUser;
                }
            });
        }
    }
}