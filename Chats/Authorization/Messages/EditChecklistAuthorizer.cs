using System.Linq;
using Chats.Commands.Messages;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Messages
{
    public class EditChecklistAuthorizer : AbstractRequestAuthorizer<EditChecklistCommand>
    {
        private readonly IChatService _chatService;
        
        public EditChecklistAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(EditChecklistCommand request)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var isMember = chat?.Members.Any(m => m.Uuid == user.Uuid);
                    return chat is not null && isMember == true;
                }
            });
        }
    }
}