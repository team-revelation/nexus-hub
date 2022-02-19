using System.Linq;
using Chats.Commands.Messages;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Messages
{
    public class EditMessageAuthorizer : AbstractRequestAuthorizer<EditMessageCommand>
    {
        private readonly IChatService _chatService;
        
        public EditMessageAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(EditMessageCommand request)
        {
            var chat = await _chatService.Get(request.ChatUuid);
            var message = chat?.Messages.FirstOrDefault(message => message.Uuid == request.MessageUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                {
                    var isSameCreator = message?.Creator == user.Uuid;
                    var isMember = chat?.Members.Any(m => m.Uuid == user.Uuid);
                    return message is not null && isSameCreator && isMember == true;
                }
            });
        }
    }
}