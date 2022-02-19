using System.Linq;
using Chats.Commands.Messages;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Messages
{
    public class SendMessageAuthorizer : AbstractRequestAuthorizer<SendMessageCommand>
    {
        private readonly IChatService _chatService;
        
        public SendMessageAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(SendMessageCommand request)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var isMessageCreator = request.Message.Creator == user.Uuid;
                    var isMember = chat.Members.Any(m => m.Uuid == user.Uuid);
                    return isMessageCreator && isMember;
                }
            });
        }
    }
}