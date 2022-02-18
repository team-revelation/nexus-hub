using System.Linq;
using Chats.Filters;
using Chats.Queries;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Chats
{
    public class GetChatAuthorizer : AbstractRequestAuthorizer<GetChatQuery>
    {
        private readonly IChatService _chatService;
        
        public GetChatAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(GetChatQuery request)
        {
            var chat = await _chatService.Get(request.Uuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => chat.Members.Any(member => member.Uuid == user.Uuid)
            });
        }
    }
}