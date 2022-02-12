using Chats.Commands;
using Chats.Filters;
using MediatR.Behaviors.Authorization;

namespace Chats.Authorization
{
    public class CreateChatAuthorizer : AbstractRequestAuthorizer<CreateChatCommand>
    {
        public override void BuildPolicy(CreateChatCommand request)
        {
            UseRequirement(new MustHaveUserRequirement
            {
                IsAuthorizedCheck = user => request.Chat.Creator == user.Uuid
            });
        }
    }
}