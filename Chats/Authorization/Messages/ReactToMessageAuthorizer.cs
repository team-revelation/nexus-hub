using System.Linq;
using Chats.Commands.Messages;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Messages
{
    public class ReactToMessageAuthorizer : AbstractRequestAuthorizer<ReactToMessageCommand>
    {
        private readonly IChatService _chatService;
        
        public ReactToMessageAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public override async void BuildPolicy(ReactToMessageCommand request)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var isSameUser = request.UserUuid == user.Uuid;
                    var isMember = chat?.Members.Any(m => m.Uuid == user.Uuid);
                    return chat is not null && isSameUser && isMember == true;
                }
            });
        }
    }
}