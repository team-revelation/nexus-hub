using System;
using System.Linq;
using Chats.Commands;
using Chats.Filters;
using Contracts.Chats;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization
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
            var message = (await _chatService.Get(request.ChatUuid)).Messages.FirstOrDefault(message => message.Uuid == request.Uuid);
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user =>
                                    {
                                        var isContentSame = message?.Content == request.Message.Content;
                                        var hasChecklist = message?.Checklists?.Count > 0;
                                        var isSameCreator = message?.Creator == user.Uuid;
                                        var isEditableChecklist = isContentSame && hasChecklist;

                                        return message is not null && (isSameCreator || isEditableChecklist);
                                    }
            });
        }
    }
}