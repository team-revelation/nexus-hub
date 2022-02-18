using System.Linq;
using Chats.Commands.Messages;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization.Messages
{
    public class EditPollAuthorizer : AbstractRequestAuthorizer<EditPollCommand>
    {
        private readonly IChatService _chatService;
        
        public EditPollAuthorizer(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async void BuildPolicy(EditPollCommand request)
        {
            var message = (await _chatService.Get(request.ChatUuid)).Messages.FirstOrDefault(message => message.Uuid == request.MessageUuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                                    {
                                        var isSameCreator = message?.Creator == user.Uuid;
                                        return message is not null && isSameCreator;
                                    }
            });
        }
    }
}